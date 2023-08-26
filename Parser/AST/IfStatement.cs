using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class IfStatement : Expression
    {
        public Expression Test = new();
        public Expression Success = new();
        public Expression? Alternate = null;

        public IfStatement()
        {
            Kind = Kind.IfStatement;
        }
    }
}
