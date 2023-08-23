using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ModifierExpression : Expression
    {
        public Runtime.Values.Modifier? Modifier = null;
        public Expression ToModify = new Expression();

        public ModifierExpression()
        {
            Kind = Kind.Modifier;
        }
    }
}
