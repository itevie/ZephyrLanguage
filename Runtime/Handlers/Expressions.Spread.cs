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
        public static RuntimeValue EvaluateSpreadExpression(SpreadExpression expression, Environment environment)
        {
            throw new RuntimeException($"Unexpected spread operator", expression.Location);
        }
    }
}
