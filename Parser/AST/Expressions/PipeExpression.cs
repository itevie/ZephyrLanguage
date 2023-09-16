using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST.Expressions
{
    internal class PipeExpression : Expression
    {
        public Expression Left { get; set; } = new Expression();
        public Expression Right { get; set; } = new CallExpression();

        public PipeExpression()
        {
            Kind = Kind.PipeExpression;
        }
    }
}
