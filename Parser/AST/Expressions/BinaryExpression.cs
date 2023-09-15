using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class BinaryExpression : Expression
    {
        public Expression Left = new();
        public Expression Right = new();
        public string Operator = "";

        public BinaryExpression()
        {
            Kind = Kind.BinaryExpression;
        }
    }
}
