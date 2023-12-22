using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class PipeExpression : Expression
    {
        public Expression Left;
        public Expression Right;

        public PipeExpression(Location location, Expression left, Expression right)
            : base(Kind.PipeExpression, location)
        {
            Left = left;
            Right = right;
        }
    }
}
