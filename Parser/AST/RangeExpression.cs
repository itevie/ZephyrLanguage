using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class RangeExpression : Expression
    {
        public Expression Left;
        public Expression Right;
        public bool Uninclusive { get; set; } = false;
        public Expression? Step { get; set; } = null;

        public RangeExpression(Location location, Expression left, Expression right)
            : base(Kind.RangeExpression, location)
        {
            Left = left;
            Right = right;
        }
    }
}
