using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Literals
    {
        public static RuntimeValue EvaluateNumericLiteral(NumericLiteral literal, Environment _)
        {
            return new NumberValue(literal.Value)
            {
                Location = literal.Location,
            };
        }
    }
}
