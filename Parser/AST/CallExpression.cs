using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class CallExpression : Expression
    {
        public List<Expression> Arguments = new();
        public Expression Caller = new Expression();

        public CallExpression()
        {
            Kind = Kind.CallExpression;
        }
    }
}
