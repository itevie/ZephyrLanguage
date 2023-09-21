using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST.Statements
{
    internal class SwitchStatement : Expression
    {
        public Expression Test { get; set; } = new();
        public List<AST.Expressions.SwitchCase> Cases { get; set; } = new();

        public SwitchStatement()
        {
            Kind = Kind.SwitchStatement;
        }
    }
}
