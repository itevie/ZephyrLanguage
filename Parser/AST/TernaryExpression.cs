using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class TernaryExpression : Expression
    {
        public Expression Success;
        public Expression Alternate;
        public Expression Test;

        public TernaryExpression(Location location, Expression success, Expression test, Expression alternate)
            : base(Kind.TernaryExpression, location)
        {
            Success = success;
            Alternate = alternate;
            Test = test;
        }
    }
}
