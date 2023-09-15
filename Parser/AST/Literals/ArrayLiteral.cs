using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ArrayLiteral : Expression
    {
        public List<Expression> Items { get; set; } = new List<Expression>();
        public Runtime.Values.ValueType Type = Runtime.Values.ValueType.Any;
        public bool IsTypeNullable = false;

        public ArrayLiteral()
        {
            Kind = Kind.ArrayLiteral;
        }
    }
}
