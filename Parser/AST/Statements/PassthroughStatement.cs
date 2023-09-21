using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST.Statements
{
    internal class PassthroughStatement : Expression
    {
        public PassthroughStatement()
        {
            Kind = Kind.PassthroughStatement;
        }
    }
}
