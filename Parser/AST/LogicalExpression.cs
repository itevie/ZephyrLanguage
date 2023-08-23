using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class LogicalExpression : Expression
    {
        public AST.Expression Right = new Expression();
        public AST.Expression Left = new Expression();
        public string Operator = "";

        public LogicalExpression()
        {
            Kind = Kind.LogicalExpression;
        }
    }
}
