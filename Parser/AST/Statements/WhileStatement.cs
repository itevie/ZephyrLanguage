using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class WhileStatement : Expression
    {
        public Expression Test { get; set; } = new Expression();
        public Expression Body { get; set; } = new Expression();

        public WhileStatement()
        {
            Kind = Kind.WhileStatement;
        }
    }
}
