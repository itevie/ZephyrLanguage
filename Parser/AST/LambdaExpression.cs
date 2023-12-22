using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class LambdaExpression : Expression
    {
        public Expression Body;
        public List<Identifier> Arguments;

        public LambdaExpression(Location location, Expression body, List<Identifier> arguments)
            : base(Kind.LambdaExpression, location)
        {
            Body = body;
            Arguments = arguments;
        }
    }
}
