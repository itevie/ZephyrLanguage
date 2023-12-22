using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateUnaryExpression(Parser.AST.UnaryExpression expression, Environment environment)
        {
            RuntimeValue value = Interpreter.Evaluate(expression.Value, environment);

            // -
            if (expression.Operator == Operators.ArithmeticOperators["Minus"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);

                return new NumberValue(-((NumberValue)value).Value);
            }

            // +
            else if (expression.Operator == Operators.ArithmeticOperators["Plus"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);

                return new NumberValue(+((NumberValue)value).Value);
            }

            // !
            else if (expression.Operator == Operators.LogicalOperators["Not"].Symbol)
            {
                return new BooleanValue(!Values.Helpers.IsValueTruthy(value));
            }

            // $
            else if (expression.Operator == Operators.BasicOperators["LengthOf"].Symbol)
            {
                return new NumberValue(value.Length(expression.Value.Location));
            }

            // typeof
            else if (expression.Operator == Operators.BasicOperators["Typeof"].Symbol)
            {
                return new StringValue(Values.Helpers.VisualiseType(value.Type));
            }

            // ++
            else if (expression.Operator == Operators.BasicOperators["Increment"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);
                NumberValue numberValue = (NumberValue)value;
                numberValue.Value++;
                return numberValue;
            }

            // --
            else if (expression.Operator == Operators.BasicOperators["Decrement"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);
                NumberValue numberValue = (NumberValue)value;
                numberValue.Value--;
                return numberValue;
            }

            else
            {
                throw new RuntimeException($"Cannot handle this unary operator", expression.Location);
            }
        }
    }
}
