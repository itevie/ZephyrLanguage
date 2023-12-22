using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue ExecuteOperator(RuntimeValue left, RuntimeValue right, string oper, bool isAssign, Expression from)
        {
            string visualOperator = $"{(isAssign ? "=" : "")}{oper}";
            string visualOperation = $"{Values.Helpers.VisualiseType(left.Type)} {visualOperator} {Values.Helpers.VisualiseType(right.Type)}";

            RuntimeException generateException(string message)
            {
                return new RuntimeException(message, from.Location);
            }

            // Numbers only
            if (
                left.Type.TypeName == Values.ValueType.Number &&
                right.Type.TypeName == Values.ValueType.Number &&
                Operators.ArithmeticOperators.Any(o => o.Value.Symbol == oper))
            {
                double leftValue = ((NumberValue)left).Value;
                double rightValue = ((NumberValue)right).Value;

                double result = 0;

                // Execute operator
                if (oper == Operators.ArithmeticOperators["Plus"].Symbol)
                    result = leftValue + rightValue;
                else if (oper == Operators.ArithmeticOperators["Minus"].Symbol)
                    result = leftValue - rightValue;
                else if (oper == Operators.ArithmeticOperators["Multiply"].Symbol)
                    result = leftValue * rightValue;
                else if (oper == Operators.ArithmeticOperators["Divide"].Symbol)
                    result = leftValue / rightValue;
                else if (oper == Operators.ArithmeticOperators["Modulo"].Symbol)
                    result = leftValue % rightValue;
                else if (oper == Operators.ArithmeticOperators["Power"].Symbol)
                    result = Math.Pow(leftValue, rightValue);
                else if (oper == Operators.ArithmeticOperators["Percentage"].Symbol)
                    result = (leftValue / 100) * rightValue;
                else if (oper == Operators.ArithmeticOperators["ReversePercentage"].Symbol)
                    result = (leftValue / rightValue) * 100;
                else if (oper == Operators.ArithmeticOperators["Exponent"].Symbol)
                    result = Math.Pow(leftValue, rightValue);

                return new NumberValue(result);
            }

            switch (left.Type.TypeName) {
                // string + ?
                case Values.ValueType.String:
                    switch (right.Type.TypeName) {
                        // string + string
                        case Values.ValueType.String:
                            return new StringValue(((StringValue)left).Value + ((StringValue)right).Value);
                        // string + boolean
                        case Values.ValueType.Boolean:
                            return new StringValue(((StringValue)left).Value + (((BooleanValue)right).Value == true ? "true" : "false"));
                        // string + null
                        case Values.ValueType.Null:
                            return new StringValue(((StringValue)left).Value + "null");
                        // string + number
                        case Values.ValueType.Number:
                            return new StringValue(((StringValue)left).Value + ((NumberValue)right).Value);
                    }
                    break;
            }

            throw generateException($"Cannot handle {visualOperation}");
        }
    }
}
