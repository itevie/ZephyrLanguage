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
        public static RuntimeValue EvaluateBinaryExpression(BinaryExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

            return Helpers.ExecuteOperator(left, right, expression.Operator, false, expression);
        }
    }
}
