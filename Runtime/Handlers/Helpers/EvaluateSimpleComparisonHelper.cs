using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static bool EvaluateSimpleComparisonHelper(RuntimeValue left, RuntimeValue right, string op, Expression? from = null)
        {
            // Generic number-only error
            string numberOnlyError = $"Can only use {op} on numbers, but got {left.Type} {op} {right.Type}";

            RuntimeException generateError(string error)
            {
                throw new RuntimeException_new()
                {
                    Location = from?.Location,
                    Error = error
                };
            }

            // These can work on any types
            // Todo Make it so it doesn't use dynamic type!
            if (op == Operators.ComparisonOperators["Equals"].Symbol)
            {
                return ((dynamic)left).Value == ((dynamic)right).Value;
            }
            
            else if (op == Operators.ComparisonOperators["NotEquals"].Symbol)
            {
                return ((dynamic)left).Value != ((dynamic)right).Value;
            }

            double leftValue;
            if (left.Type == Values.ValueType.Int)
                leftValue = ((IntegerValue)left).Value;
            else if (right.Type == Values.ValueType.Float) leftValue = ((FloatValue)left).Value;
            else if (right.Type == Values.ValueType.Long) leftValue = ((LongValue)left).Value;
            else throw new Exception("insert error here");

            double rightValue;
            if (right.Type == Values.ValueType.Int)
                rightValue = ((IntegerValue)right).Value;
            else if (right.Type == Values.ValueType.Float) rightValue = ((FloatValue)right).Value;
            else if (right.Type == Values.ValueType.Long) rightValue = ((LongValue)right).Value;
            else throw new Exception("insert error here");

            // Only numbers
            if (op == Operators.ComparisonOperators["LessThan"].Symbol)
            {
                return leftValue < rightValue;
            }

            else if (op == Operators.ComparisonOperators["GreaterThan"].Symbol)
            {
                return leftValue > rightValue;
            }

            else if (op == Operators.ComparisonOperators["LessThanOrEquals"].Symbol)
            {
                return leftValue <= rightValue;
            }

            else if (op == Operators.ComparisonOperators["GreaterThanOrEquals"].Symbol)
            {
                return leftValue >= rightValue;
            }

            else
            {
                throw generateError($"Unexpected operator: {op}");
            }
        }
    }
}
