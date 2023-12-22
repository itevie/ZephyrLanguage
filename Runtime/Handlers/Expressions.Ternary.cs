using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateTernaryExpression(TernaryExpression expression, Environment environment)
        {
            // Check test
            RuntimeValue test = Interpreter.Evaluate(expression.Test, environment);
            bool success = Values.Helpers.IsValueTruthy(test);

            if (success)
                return Interpreter.Evaluate(expression.Success, environment);
            else return Interpreter.Evaluate(expression.Alternate, environment);
        }
    }
}
