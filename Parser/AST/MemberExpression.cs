using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class MemberExpression : Expression
    {
        public Expression Left;
        public Expression Right;
        public bool IsComputed = false;

        public MemberExpression(Location location, Expression left, Expression right)
            : base(Kind.MemberExpression, location)
        {
            Left = left;
            Right = right;
        }
    }
}
