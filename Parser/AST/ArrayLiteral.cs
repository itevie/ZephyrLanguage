using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ArrayLiteral : Expression
    {
        public List<Expression> Items = new List<Expression>();

        public ArrayLiteral(Location location, List<Expression> items)
            : base(Kind.ArrayLiteral, location)
        {
            Items = items;
        }
    }
}
