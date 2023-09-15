using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;
using Zephyr.Parser;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateAssignmentExpression(Parser.AST.AssignmentExpression node, Environment environment)
        {
            // Check if allowed to assign to this
            if (node.Assignee.Kind != Parser.AST.Kind.Identifier && node.Assignee.Kind != Parser.AST.Kind.MemberExpression)
            {
                throw new RuntimeException(new()
                {
                    Location = node.Assignee.Location,
                    Error = $"Cannot assign to {node.Assignee.Kind}"
                });
            }

            // Temporary error
            if (node.Assignee.Kind == Parser.AST.Kind.MemberExpression)
            {
                throw new RuntimeException(new()
                {
                    Location = node.Assignee.Location,
                    Error = $"Cannot assign to member expressions yet, use objets.set or arrays.set"
                });
            }

            // Get the values
            string variableName = ((Parser.AST.Identifier)node.Assignee).Symbol;
            RuntimeValue givenNewValue = node.Value != null
                ? Interpreter.Evaluate(node.Value, environment)
                : Values.Helpers.Helpers.CreateNull();

            RuntimeValue newValue = Values.Helpers.Helpers.CreateNull();
            RuntimeValue oldValue = environment.LookupVariable(variableName, node.Assignee);

            // Check the type
            if (node.Type == Operators.AssignmentOperators["CoalesenceAssignment"].Symbol)
            {
                if (oldValue.Type == Values.ValueType.Null)
                {
                    newValue = givenNewValue;
                } else
                {
                    return oldValue;
                }
            }

            // It is a direct assignment a = b
            else if (node.Type == Operators.AssignmentOperators["NormalAssignment"].Symbol)
            {
                newValue = givenNewValue;
            }

            // It is a simple type
            else
            {
                // Check for event type as it can only have += or -=
                if (oldValue.Type == Values.ValueType.Event)
                {
                    // +=
                    if (node.Type == Operators.AssignmentOperators["PlusAssignment"].Symbol)
                    {
                        // Check right side is function
                        if (givenNewValue.Type != Values.ValueType.Function)
                        {
                            throw new RuntimeException(new()
                            {
                                Error = $"Can only event += function, got event += {givenNewValue.Type}",
                                Location = node.Location,
                            });
                        }

                        // Add listener
                        ((EventValue)oldValue).AddListener((FunctionValue)givenNewValue);
                        return Values.Helpers.Helpers.CreateNull();
                    } else if (node.Type == Operators.AssignmentOperators["SubtractAssignment"].Symbol)
                    {
                        // Check right side is function
                        if (givenNewValue.Type != Values.ValueType.Function)
                        {
                            throw new RuntimeException(new()
                            {
                                Error = $"Can only event -= function, got event -= {givenNewValue.Type}",
                                Location = node.Location,
                            });
                        }

                        // Remove listener
                        ((EventValue)oldValue).RemoveListener((FunctionValue)givenNewValue);
                        return Values.Helpers.Helpers.CreateNull();
                    } else
                    {
                        throw new RuntimeException(new()
                        {
                            Error = $"Cannot use {node.Type} on event",
                            Location = node.Location,
                        });
                    }
                } else
                {
                    newValue = Helpers.ExecuteOperatorHelper(oldValue, givenNewValue, node.Type.Replace("=", ""), true, node);
                }
            }

            return environment.AssignVariable(variableName, newValue, false, node);
        }
    }
}
