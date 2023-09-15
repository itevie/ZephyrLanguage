using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateTernaryExpression(TernaryExpression ternaryExpression, Environment environment)
        {
            // Evaluate test
            RuntimeValue test = Interpreter.Evaluate(ternaryExpression.Test, environment);
            bool isSuccess = Helpers.EvaluateTruhyValueHelper(test);

            if (isSuccess)
            {
                return Interpreter.Evaluate(ternaryExpression.Success, environment);
            } else
            {
                return Interpreter.Evaluate(ternaryExpression.Alternate, environment);
            }
        }
    }
}
