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
        private static List<string> _simpleOperators = new List<string>()
        {
            Operators.ComparisonOperators["Equals"].Symbol,
            Operators.ComparisonOperators["NotEquals"].Symbol,
            Operators.ComparisonOperators["GreaterThan"].Symbol,
            Operators.ComparisonOperators["GreaterThanOrEquals"].Symbol,
            Operators.ComparisonOperators["LessThan"].Symbol,
            Operators.ComparisonOperators["LessThanOrEquals"].Symbol,
        };

        public static RuntimeValue EvaluateComparisonExpression(Parser.AST.ComparisonExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);
            string op = expression.Operator;

            // Check if types are the same
            if (left.Type != right.Type)
                throw new RuntimeException(new()
                {
                    Location = expression.Location,
                    Error = $"Can only compare values with same type, got {left.Type} {op} {right.Type}"
                });

            bool isEqual = false;

            void evaluateSimple()
            {
                if (_simpleOperators.Contains(expression.Operator) == false)
                    throw new Exception($"Cannot use the operator {op} on this type, got {left.Type} {op} {right.Type}");
                isEqual = Helpers.EvaluateSimpleComparisonHelper(left, right, op);
            }

            switch (left.Type)
            {
                case Values.ValueType.Int:
                    // Multicompare
                    if (op == Operators.ComparisonOperators["MultiCompare"].Symbol)
                    {
                        IntegerValue l = (IntegerValue)left;
                        IntegerValue r = (IntegerValue)right;

                        if (l.Value == r.Value)
                            return Values.Helpers.Helpers.CreateInteger(0);
                        else if (l.Value > r.Value)
                            return Values.Helpers.Helpers.CreateInteger(1);
                        else if (l.Value < r.Value)
                            return Values.Helpers.Helpers.CreateInteger(-1);
                    } else
                    {
                        evaluateSimple();
                    }
                    break;
                case Values.ValueType.Boolean:
                case Values.ValueType.Null:
                case Values.ValueType.String:
                    evaluateSimple();
                    break;
                default:
                    throw new Exception($"Cannot use comparison operators on type {left.Type}");
            }

            return Values.Helpers.Helpers.CreateBoolean(isEqual);
        }
    }
}
