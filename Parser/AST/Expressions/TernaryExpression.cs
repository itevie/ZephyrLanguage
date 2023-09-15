using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class TernaryExpression : Expression
    {
        public Expression Test { get; set; } = new Expression();
        public Expression Success { get; set; } = new Expression();
        public Expression Alternate { get; set; } = new Expression();

        public TernaryExpression()
        {
            Kind = Kind.TernaryExpression;
        }
    }
}
