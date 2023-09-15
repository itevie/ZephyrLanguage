using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ReturnStatement : Expression
    {
        public Expression? Value { get; set; } = null;

        public ReturnStatement()
        {
            Kind = Kind.ReturnStatement;
        }
    }
}
