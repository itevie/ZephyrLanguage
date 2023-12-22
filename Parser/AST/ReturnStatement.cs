using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ReturnStatement : Expression
    {
        public Expression Value;

        public ReturnStatement(Location location, Expression value)
            : base(Kind.ReturnStatement, location)
        {
            Value = value;
        }
    }
}
