using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Parser.AST
{
    internal class TypeNameCombo
    {
        public Type Type;
        public Identifier Identifier;

        public TypeNameCombo(Type type, Identifier identifier)
        {
            Type = type;
            Identifier = identifier;
        }
    }
}
