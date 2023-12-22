using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime
{
    internal class Export
    {
        public RuntimeValue Value;
        public string Identifier;
        public Location ExportedAt;

        public Export(RuntimeValue value, string identifier, Location exportedAt)
        {
            Value = value;
            Identifier = identifier;
            ExportedAt = exportedAt;
        }
    }
}
