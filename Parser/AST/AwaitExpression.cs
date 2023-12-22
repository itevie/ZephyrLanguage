using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class AwaitExpression : Expression
    {
        public Expression Right;
        
        public AwaitExpression(Location location, Expression right)
            : base(Kind.AwaitExpression, location)
        {
            Right = right;
        }
    }
}
