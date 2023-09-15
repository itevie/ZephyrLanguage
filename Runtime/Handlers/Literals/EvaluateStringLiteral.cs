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
        public static Values.RuntimeValue EvaluateStringLiteral(Parser.AST.StringLiteral stringLiteral, Environment environment)
        {
            RuntimeValue value = Values.Helpers.Helpers.CreateString(stringLiteral.Value);
            value.Location = stringLiteral.Location;
            return value;
        }
    }
}
