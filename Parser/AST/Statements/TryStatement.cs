using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class TryStatement : Expression
    {
        public Expression Body { get; set; } = new Expression();
        public Expression? CatchBody { get; set; } = null;
        public Expression? IdentifierToCreate { get; set; } = null;

        public TryStatement()
        {
            Kind = Kind.TryStatement;
        }
    }
}
