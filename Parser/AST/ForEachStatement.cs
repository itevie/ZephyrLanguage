using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ForEachStatement : Expression
    {
        public Expression Identifier { get; set; } = new Expression();
        public Expression ValueToEnumerate { get; set; } = new Expression();
        public Expression Body { get; set; } = new Expression();

        public ForEachStatement()
        {
            Kind = Kind.ForEachStatement;
        }
    }
}
