using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class NumericLiteral : Expression
    {
        public float Value { get; set; } = 0;
        public bool IsFloat { get; set; } = false;

        public NumericLiteral()
        {
            Kind = Kind.NumericLiteral;
        }
    }
}
