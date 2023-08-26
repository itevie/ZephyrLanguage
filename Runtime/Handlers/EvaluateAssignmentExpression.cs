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

            if (node.Assignee.Kind == Parser.AST.Kind.MemberExpression)
            {
                throw new RuntimeException(new()
                {
                    Location = node.Assignee.Location,
                    Error = $"Cannot assign to member expressions yet, use objets.set or arrays.set"
                });
            }

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
            else if (node.Type == Operators.AssignmentOperators["NormalAssignment"].Symbol)
            {
                newValue = givenNewValue;
            }

            // It is a simple type
            else
            {
                newValue = Helpers.ExecuteOperatorHelper(oldValue, givenNewValue, node.Type.Replace("=", ""), true, node);
            }

            return environment.AssignVariable(variableName, newValue, false, node);
        }
    }
}
