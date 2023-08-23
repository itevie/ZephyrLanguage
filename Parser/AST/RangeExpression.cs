using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class RangeExpression : Expression
    {
        public Expression Start { get; set; } = new Expression();
        public Expression End { get; set; } = new Expression();
        public Expression? Step { get; set; } = null;
        public bool Uninclusive { get; set; } = false;
        
        public RangeExpression()
        {
            Kind = Kind.RangeExpression;
        }
    }
}
