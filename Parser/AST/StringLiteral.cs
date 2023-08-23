using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class StringLiteral : Expression
    {
        public string Value = "";

        public StringLiteral()
        {
            Kind = Kind.StringLiteral;
        }
    }
}
