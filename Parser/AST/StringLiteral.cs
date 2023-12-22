using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class StringLiteral : Expression
    {
        public string Value;

        public StringLiteral(Location location, string value) : base(Kind.StringLiteral, location)
        {
            Value = value;
        }
    }
}
