using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class BreakStatement : Expression
    {
        public BreakStatement()
        {
            Kind = Kind.BreakStatement;
        }
    }
}
