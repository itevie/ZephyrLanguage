using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class LoopStatement : Expression
    {
        public Expression Body { get; set; }
        
        public LoopStatement(Location location, Expression body)
            : base(Kind.LoopStatement, location)
        {
            NeedsSemicolon = false;
            Body = body;
        }
    }
}
