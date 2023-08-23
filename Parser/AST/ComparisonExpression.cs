using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ComparisonExpression : Expression
    {
        public AST.Expression Right = new Expression();
        public AST.Expression Left = new Expression();
        public string Operator = "";

        public ComparisonExpression() 
        {
            Kind = Kind.ComparisonExpression;
        }
    }
}
