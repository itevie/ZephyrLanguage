using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateUnaryRightExpression(UnaryRightExpression expression, Environment environment)
        {
            RuntimeValue value = Interpreter.Evaluate(expression.Value, environment);

            // ++
            if (expression.Operator == Operators.BasicOperators["Increment"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);
                NumberValue numberValue = (NumberValue)value;
                NumberValue oldValue = new NumberValue(numberValue.Value);
                numberValue.Value++;
                return oldValue;
            }

            // --
            else if (expression.Operator == Operators.BasicOperators["Decrement"].Symbol)
            {
                // Check if number
                if (value.Type.TypeName != Values.ValueType.Number)
                    throw new RuntimeException($"{expression.Operator} can only be used with numbers", expression.Value.Location);
                NumberValue numberValue = (NumberValue)value;
                NumberValue oldValue = new NumberValue(numberValue.Value);
                numberValue.Value--;
                return oldValue;
            }

            else
            {
                throw new RuntimeException($"Cannot handle this unary operator", expression.Location);
            }
        }
    }
}
