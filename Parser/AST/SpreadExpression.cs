using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class SpreadExpression : Expression
    {
        public Expression Right;

        public SpreadExpression(Location location, Expression right)
            : base(Kind.SpreadExpression, location)
        {
            Right = right;
        }
    }
}
