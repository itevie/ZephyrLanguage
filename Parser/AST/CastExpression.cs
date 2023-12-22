using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class CastExpression : Expression
    {
        public Expression Value;
        public AST.Type CastTo;

        public CastExpression(Location location, Expression value, AST.Type castTo)
            : base(Kind.CastExpression, location)
        {
            Value = value;
            CastTo = castTo;
        }
    }
}
