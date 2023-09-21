using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue ExecuteOperatorHelper(RuntimeValue left, RuntimeValue right, string op, bool isAssign = false, Expression? from = null)
        {
            string visualOperator = $"{op}{(isAssign ? "=" : "")}";
            string fullOperation = $"{left.Type} {visualOperator} {right.Type}";

            RuntimeException generateError(string error)
            {
                throw new RuntimeException_new()
                {
                    Location = from?.Location,
                    Error = error,
                };
            }

            // Numbers only
            Values.ValueType[] values = { Values.ValueType.Int, Values.ValueType.Long, Values.ValueType.Float };

            if (values.Contains(left.Type) && values.Contains(right.Type) && Lexer.Syntax.Operators.ArithmeticOperators.Any(x => x.Value.Symbol == op))
            {
                double leftValue = 0;
                double rightValue = 0;

                // This defines what type the finished result will be
                Values.ValueType finishingType = Values.ValueType.Null;

                // The higher the type is what will be converted, e.g. int + float, finishingType will = flaot
                void setMax(Values.ValueType val)
                {
                    if ((int)finishingType < (int)val)
                    {
                        finishingType = val;
                    }
                }

                setMax(left.Type);
                setMax(right.Type);

                // Get the left's value
                switch (left.Type)
                {
                    case Values.ValueType.Int:
                        leftValue = ((IntegerValue)left).Value;
                        break;
                    case Values.ValueType.Long:
                        leftValue = ((LongValue)left).Value;
                        break;
                    case Values.ValueType.Float:
                        leftValue = ((FloatValue)left).Value;
                        break;
                }

                // Get the right's value
                switch (right.Type)
                {
                    case Values.ValueType.Int:
                        rightValue = ((IntegerValue)right).Value;
                        break;
                    case Values.ValueType.Long:
                        rightValue = ((LongValue)right).Value;
                        break;
                    case Values.ValueType.Float:
                        rightValue = ((FloatValue)right).Value;
                        break;
                }

                // Do the operation
                double result = 0;

                if (op == Lexer.Syntax.Operators.ArithmeticOperators["Plus"].Symbol)
                {
                    result = leftValue + rightValue;
                } else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Subtract"].Symbol)
                {
                    result = leftValue - rightValue;
                } else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Multiply"].Symbol)
                {
                    result = leftValue * rightValue;
                } else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Divide"].Symbol)
                {
                    result = leftValue / rightValue;
                } else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Power"].Symbol)
                {
                    result = Math.Pow(leftValue, rightValue);
                } else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Modulus"].Symbol)
                {
                    result = leftValue % rightValue;
                }

                // Output value
                return finishingType switch
                {
                    Values.ValueType.Int => Values.Helpers.Helpers.CreateInteger((int)result),
                    Values.ValueType.Float => Values.Helpers.Helpers.CreateFloat(result),
                    Values.ValueType.Long => Values.Helpers.Helpers.CreateLongValue((long)result),
                    _ => throw new Exception(""),
                };
            }

            if (op == Lexer.Syntax.Operators.ArithmeticOperators["Plus"].Symbol)
            {
                switch (left.Type)
                {
                    case Values.ValueType.String:
                        // Expect same type
                        if (right.Type != Values.ValueType.String)
                        {
                            // Try convert to string
                            return CastValueHelper(right, left.Type, from?.Location);
                            //throw generateError($"Can only {visualOperator} a string to another string, got {fullOperation}");
                        }
                        return Values.Helpers.Helpers.CreateString(((StringValue)left).Value + ((StringValue)right).Value);
                    case Values.ValueType.Array:
                        ArrayValue arrValue = (ArrayValue)left;

                        // Check if allowed to add
                        if (arrValue.ItemsType != right.Type && arrValue.ItemsType != Values.ValueType.Any)
                            throw generateError($"Cannot append type {right.Type} to a {arrValue.ItemsType} array");
                        
                        // Add item
                        arrValue.Items.Add(right);
                        return Values.Helpers.Helpers.CreateArray(arrValue.Items, arrValue.ItemsType);
                }
            }

            else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Subtract"].Symbol)
            {
                switch (left.Type)
                {

                }
            }

            else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Multiply"].Symbol)
            {
                switch (left.Type)
                {
                    case Values.ValueType.String:
                        // Expect "" * number
                        if (right.Type != Values.ValueType.Int)
                            throw generateError($"Can only {visualOperator} a string to a number, got {fullOperation}");
                        return Values.Helpers.Helpers.CreateString(string.Concat(Enumerable.Repeat(((StringValue)left).Value, (int)((IntegerValue)right).Value)));
                }
            }

            else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Divide"].Symbol)
            {
                switch (left.Type)
                {

                }
            }

            else if (op == Lexer.Syntax.Operators.BinaryOperators["Coalesence"].Symbol)
            {
                if (left.Type == Values.ValueType.Null)
                {
                    return right;
                }
                else return left;
            }

            throw generateError($"Cannot handle {left.Type} {op} {right.Type}");
        }
    }
}
