using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class BlockStatement : Expression
    {
        public List<Expression> Body = new();

        public BlockStatement()
        {
            Kind = Kind.BlockStatement;
        }
    }
}
