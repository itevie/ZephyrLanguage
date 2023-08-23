using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class UnaryExpression : Expression
    {
        public AST.Expression Right = new AST.Expression();
        public string Operator = "";

        public UnaryExpression()
        {
            Kind = Kind.UnaryExpression;
        }
    }
}
