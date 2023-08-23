using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class IfStatement : Expression
    {
        public Expression Test = new Expression();
        public Expression Success = new Expression();
        public Expression? Alternate = null;

        public IfStatement()
        {
            Kind = Kind.IfStatement;
        }
    }
}
