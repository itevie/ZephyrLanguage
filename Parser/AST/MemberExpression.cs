using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class MemberExpression : Expression
    {
        public AST.Expression Object = new AST.Expression();
        public AST.Expression Property = new AST.Expression();
        public bool IsComputed = false;

        public MemberExpression()
        {
            Kind = Kind.MemberExpression;
        }
    }
}
