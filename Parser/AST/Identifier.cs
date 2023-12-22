using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class Identifier : Expression
    {
        public string Symbol;

        public Identifier(Location location, string symbol) : base(Kind.Identifier, location)
        {
            Symbol = symbol;
        }
    }
}
