using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateComparisonExpression(ComparisonExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);
            string oper = expression.Operator;

            string operation = $"{left.Type.TypeName} {oper} {right.Type.TypeName}";

            // Checking operators
            if (oper == Operators.ComparisonOperators["Equals"].Symbol)
            {
                // Check if values are the same
                if (!Values.Helpers.CompareTypes(left.Type, right.Type))
                    throw new RuntimeException($"Left type is not the same as the right type {operation}", expression.Location);


                // TODO: Make this not use a dynamic type
                try
                {
                    return new BooleanValue(((dynamic)left).Value == ((dynamic)right).Value);
                }
                catch
                {
                    throw new RuntimeException($"Failed to compare {operation}", expression.Location);
                }
            }
            else if (oper == Operators.ComparisonOperators["NotEquals"].Symbol)
            {
                // Check if values are the same
                if (!Values.Helpers.CompareTypes(left.Type, right.Type))
                    throw new RuntimeException($"Left type is not the same as the right type {operation}", expression.Location);


                // TODO: Make this not use a dynamic type
                try
                {
                    return new BooleanValue(((dynamic)left).Value != ((dynamic)right).Value);
                }
                catch
                {
                    throw new RuntimeException($"Failed to compare {operation}", expression.Location);
                }
            }
            else if (
                oper == Operators.ComparisonOperators["GreaterThan"].Symbol ||
                oper == Operators.ComparisonOperators["GreaterThanOrEquals"].Symbol ||
                oper == Operators.ComparisonOperators["LessThan"].Symbol ||
                oper == Operators.ComparisonOperators["LessThanOrEquals"].Symbol
            )
            {
                // Check types
                if (left.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"Expected number {oper} number", expression.Left.Location);
                if (right.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"Expected number {oper} number", expression.Right.Location);

                // Collect values
                double leftNumber = ((NumberValue)left).Value;
                double rightNumber = ((NumberValue)right).Value;

                // Compare
                if (oper == Operators.ComparisonOperators["GreaterThan"].Symbol)
                    return new BooleanValue(leftNumber > rightNumber);
                else if (oper == Operators.ComparisonOperators["GreaterThanOrEquals"].Symbol)
                    return new BooleanValue(leftNumber >= rightNumber);
                else if (oper == Operators.ComparisonOperators["LessThan"].Symbol)
                    return new BooleanValue(leftNumber < rightNumber);
                else
                    return new BooleanValue(leftNumber <= rightNumber);
            }
            else
            {
                throw new RuntimeException($"Cannot compare {operation}", expression.Location);
            }
        }
    }
}
