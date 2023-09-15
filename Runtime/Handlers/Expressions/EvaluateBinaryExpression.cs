using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateBinaryExpression(Parser.AST.BinaryExpression binaryExpression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(binaryExpression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(binaryExpression.Right, environment);

            return Helpers.ExecuteOperatorHelper(left, right, binaryExpression.Operator, false, binaryExpression);
        }
    }
}
