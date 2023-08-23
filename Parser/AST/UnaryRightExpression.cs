using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class UnaryRightExpression : Expression
    {
        public AST.Expression Left = new AST.Expression();
        public string Operator = "";

        public UnaryRightExpression()
        {
            Kind = Kind.UnaryRightExpression;
        }
    }
}
