using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;
using Zephyr.Parser;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        private static Dictionary<string, Operator> _unaryOperators = Operators.UnaryOperators;

        public static RuntimeValue EvaluateUnaryExpression(Parser.AST.UnaryExpression expression, Environment environment)
        {
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

            if (expression.Operator == _unaryOperators["LengthOf"].Symbol)
            {
                switch (right.Type)
                {
                    case Values.ValueType.String:
                        return Values.Helpers.Helpers.CreateInteger(((StringValue)right).Value.Length);
                    case Values.ValueType.Array:
                        return Values.Helpers.Helpers.CreateInteger(((ArrayValue)right).Items.Count);
                    case Values.ValueType.Object:
                        return Values.Helpers.Helpers.CreateInteger(((ObjectValue)right).Properties.Count);
                    default:
                        throw new RuntimeException(new()
                        {
                            Location = expression.Location,
                            Error = $"Cannot use this unary operator with types of {right.Type}"
                        });
                }
            }
            else if (expression.Operator == Operators.LogicalOperators["Not"].Symbol)
            {
                bool isTruthy = Helpers.EvaluateTruhyValueHelper(right);
                return Values.Helpers.Helpers.CreateBoolean(!isTruthy);
            }
            else
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Location,
                    Error = $"This unary operator is not yet implemented"
                });
            }
        }
    }
}
