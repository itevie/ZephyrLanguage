using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ForStatement : Expression
    {
        public Expression Declaration;
        public Expression Test;
        public Expression Increment;
        public Expression Body;

        public ForStatement(Location location, Expression declaration, Expression test, Expression increment, Expression body)
            : base(Kind.ForStatement, location)
        {
            NeedsSemicolon = false;
            Declaration = declaration;
            Test = test;
            Increment = increment;
            Body = body;
        }
    }
}
