using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST.Expressions
{
    internal class SwitchCase : Expression
    {
        public Expression Test { get; set; } = new();
        public Expression Success { get; set; } = new();

        public SwitchCase()
        {
            Kind = Kind.SwitchCase;
        }
    }
}
