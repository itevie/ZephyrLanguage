using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static Values.RuntimeValue EvaluateIdentifier(Parser.AST.Identifier identifier, Environment environment)
        {
            Values.RuntimeValue value = environment.LookupVariable(identifier.Symbol, identifier);
            if (value == null)
            {
                throw new RuntimeException(new()
                {
                    Error = "unknown error"
                });
            }
            value.Location = identifier?.Location;
            return value;
        }
    }
}
