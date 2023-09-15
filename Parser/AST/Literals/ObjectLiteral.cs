using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ObjectLiteral : Expression
    {
        public List<Property> Properties = new();

        public ObjectLiteral()
        {
            Kind = Kind.ObjectLiteral;
        }
    }
}
