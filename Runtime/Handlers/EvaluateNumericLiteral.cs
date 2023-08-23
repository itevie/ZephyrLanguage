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
        public static Values.RuntimeValue EvaluateNumericLiteral(Parser.AST.NumericLiteral numericLiteral, Environment environment)
        {
            RuntimeValue value = numericLiteral.IsFloat
                ? Values.Helpers.Helpers.CreateFloat((float)numericLiteral.Value)
                : Values.Helpers.Helpers.CreateInteger((int)numericLiteral.Value);

            value.Location = numericLiteral.Location;
            return value;
        }
    }
}
