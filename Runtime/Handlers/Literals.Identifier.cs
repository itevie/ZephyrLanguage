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
        public static RuntimeValue EvaluateIdentifier(Identifier identifier, Environment environment)
        {
            string name = identifier.Symbol;

            RuntimeValue val = environment.LookupVariable(name, identifier.Location);
            val.Location = identifier.Location;
            return val;
        }
    }
}
