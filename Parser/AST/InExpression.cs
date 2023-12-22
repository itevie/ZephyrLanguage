using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class InExpression : Expression
    {
        public Expression Left;
        public Expression Right;

        public InExpression(Location location, Expression left, Expression right)
            : base(Kind.InExpression, location)
        {
            Left = left;
            Right = right;
        }
    }
}
