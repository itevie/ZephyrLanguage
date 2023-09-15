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
    internal partial class Expressions 
    {
        public static RuntimeValue EvaluateUnaryRightExpression(UnaryRightExpression expression, Environment environment)
        {
            RuntimeValue tempLeft = Interpreter.Evaluate(expression.Left, environment);

            // Check type
            if (tempLeft.Type != Values.ValueType.Int)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Location,
                    Error = $"Cannot use unary-right expressions on a {tempLeft.Type}"
                });
            }

            IntegerValue left = (IntegerValue)tempLeft;

            if (expression.Operator == Operators.UnaryOperators["Increment"].Symbol)
                left.Value++;
            else if (expression.Operator == Operators.UnaryOperators["Decrement"].Symbol)
                left.Value--;

            return left;
        }
    }
}
