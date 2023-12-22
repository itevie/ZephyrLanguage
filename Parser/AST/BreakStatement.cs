using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class BreakStatement : Expression
    {
        public BreakStatement(Location location)
            : base(Kind.BreakStatement, location)
        {
            NeedsSemicolon = true;
        }
    }
}
