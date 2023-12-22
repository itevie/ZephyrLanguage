using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class BlockStatement : Expression
    {
        public List<Expression> Body;

        public BlockStatement(Location location, List<Expression> expressions)
            : base(Kind.BlockStatement, location)
        {
            Body = expressions;
        }
    }
}
