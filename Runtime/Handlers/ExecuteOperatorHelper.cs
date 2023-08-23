using System;
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
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = error,
                });
            }

            if (op == Lexer.Syntax.Operators.ArithmeticOperators["Plus"].Symbol)
            {
                switch (left.Type)
                {
                    case Values.ValueType.Int:
                        // Expect 1 + 1
                        if (right.Type != Values.ValueType.Int)
                            throw generateError($"Can only {visualOperator} a number to another number, got {fullOperation}");
                        return Values.Helpers.Helpers.CreateInteger(((IntegerValue)left).Value + ((IntegerValue)right).Value);
                    case Values.ValueType.String:
                        // Expect same type
                        if (right.Type != Values.ValueType.String)
                            throw generateError($"Can only {visualOperator} a string to another string, got {fullOperation}");
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
                    case Values.ValueType.Int:
                        // Expect 1 - 1
                        if (right.Type != Values.ValueType.Int)
                            throw generateError($"Can only {visualOperator} a number to another number, got {fullOperation}");
                        return Values.Helpers.Helpers.CreateInteger(((IntegerValue)left).Value - ((IntegerValue)right).Value);
                }
            }

            else if (op == Lexer.Syntax.Operators.ArithmeticOperators["Multiply"].Symbol)
            {
                switch (left.Type)
                {
                    case Values.ValueType.Int:
                        // Expect 1 * 1
                        if (right.Type != Values.ValueType.Int)
                            throw generateError($"Can only {visualOperator} a number to another number, got {fullOperation}");
                        return Values.Helpers.Helpers.CreateInteger(((IntegerValue)left).Value * ((IntegerValue)right).Value);
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
                    case Values.ValueType.Int:
                        // Expect 1 / 1
                        if (right.Type != Values.ValueType.Int)
                            throw generateError($"Can only {visualOperator} a number to another number, got {left.Type} {visualOperator} {right.Type}");
                        return Values.Helpers.Helpers.CreateInteger(((IntegerValue)left).Value / ((IntegerValue)right).Value);
                }
            }

            throw generateError($"Cannot handle {left.Type} {op} {right.Type}");
        }
    }
}
