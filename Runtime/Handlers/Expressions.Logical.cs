using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateLogicalExpression(LogicalExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

            bool leftTruthy = Values.Helpers.IsValueTruthy(left);
            bool rightTruthy = Values.Helpers.IsValueTruthy(right);

            // AND &&
            if (expression.Operator == Operators.LogicalOperators["And"].Symbol)
            {
                return new BooleanValue(leftTruthy && rightTruthy);
            }

            // NAND !(&&)
            else if (expression.Operator == Operators.LogicalOperators["Nand"].Symbol)
            {
                return new BooleanValue(!(leftTruthy && rightTruthy));
            }

            // NOR !(||)
            else if (expression.Operator == Operators.LogicalOperators["Nor"].Symbol)
            {
                return new BooleanValue(!(leftTruthy || rightTruthy));
            }

            // OR ||
            else if (expression.Operator == Operators.LogicalOperators["Or"].Symbol)
            {
                return new BooleanValue(leftTruthy || rightTruthy);
            }

            // \XOR (||) && !(&&)
            else if (expression.Operator == Operators.LogicalOperators["XOR"].Symbol)
            {
                return new BooleanValue((leftTruthy || rightTruthy) && !(leftTruthy && rightTruthy));
            }

            else
            {
                throw new RuntimeException($"Cannot use this operator", expression.Location);
            }
        }
    }
}
