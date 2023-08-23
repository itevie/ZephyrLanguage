using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class Identifier : Expression
    {
        public string Symbol { get; set; } = "";

        public Identifier()
        {
            Kind = Kind.Identifier;
        }
    }
}
