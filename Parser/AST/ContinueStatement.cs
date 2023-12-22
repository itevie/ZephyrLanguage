using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ContinueStatement : Expression
    {
        public ContinueStatement(Location location)
            : base(Kind.ContinueStatement, location)
        {
            NeedsSemicolon = true;
        }
    }
}
