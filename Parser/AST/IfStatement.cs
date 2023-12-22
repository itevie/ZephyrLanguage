using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class IfStatement : Expression
    {
        public Expression Test;
        public Expression Success;
        public Expression? Alternate;

        public IfStatement(Location location, Expression test, Expression success) 
            : base(Kind.IfStatement, location)
        {
            NeedsSemicolon = false;
            Test = test;
            Success = success;
            Alternate = null;
        }
    }
}
