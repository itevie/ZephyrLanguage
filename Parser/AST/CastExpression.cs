using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class CastExpression : Expression
    {
        public Expression Left { get; set; } = new Expression();
        public Runtime.Values.ValueType Type { get; set; } = Runtime.Values.ValueType.Any;

        public CastExpression()
        {
            Kind = Kind.CastExpression;
        }
    }
}
