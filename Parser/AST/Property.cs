using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class Property : Expression
    {
        public string Key { get; set; } = "";
        public Expression? Value { get; set; } = null;
        public bool IsAlone = false;

        public Property()
        {
            Kind = Kind.Property;
        }
    }
}
