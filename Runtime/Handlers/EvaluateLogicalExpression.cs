using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateLogicalExpression(LogicalExpression expr, Environment env)
        {
            RuntimeValue left = Interpreter.Evaluate(expr.Left, env);
            RuntimeValue right = Interpreter.Evaluate(expr.Right, env);

            bool leftTruthy = Helpers.EvaluateTruhyValueHelper(left);
            bool rightTruthy = Helpers.EvaluateTruhyValueHelper(right);

            if (expr.Operator == Operators.LogicalOperators["And"].Symbol)
            {
                if (leftTruthy && rightTruthy)
                {
                    return Values.Helpers.Helpers.CreateBoolean(true);
                }
            } else if (expr.Operator == Operators.LogicalOperators["Or"].Symbol)
            {
                if (leftTruthy || rightTruthy)
                {
                    return Values.Helpers.Helpers.CreateBoolean(true);
                }
            } else if (expr.Operator == Operators.LogicalOperators["XOR"].Symbol)
            {
                if ((leftTruthy || rightTruthy) && !(leftTruthy && rightTruthy))
                {
                    return Values.Helpers.Helpers.CreateBoolean(true);
                }
            }

            return Values.Helpers.Helpers.CreateBoolean(false);
        }
    }
}
