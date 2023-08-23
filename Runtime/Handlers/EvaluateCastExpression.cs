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
        public static RuntimeValue EvaluateCastExpression(CastExpression expression, Environment environment)
        {
            return Helpers.CastValueHelper(Interpreter.Evaluate(expression.Left, environment), expression.Type, expression);
        }
    }
}
