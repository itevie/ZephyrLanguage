using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class StructDeclaration : Expression
    {
        public Dictionary<Identifier, Type> Fields;
        public Identifier Name;

        public StructDeclaration(Location location, Identifier name, Dictionary<Identifier, Type> fields)
            : base(Kind.StructDeclaration, location)
        {
            NeedsSemicolon = false;
            Fields = fields;
            Name = name;
        }
    }
}
