using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class EchoStatement : Expression
    {
        public Expression Value;

        public EchoStatement(Location location, Expression what)
            : base(Kind.EchoStatement, location)
        {
            NeedsSemicolon = true;
            Value = what;
        }
    }
}
