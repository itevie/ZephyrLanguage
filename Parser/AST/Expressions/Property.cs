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

        /// <summary>
        /// This determines whether the value is inferred, e.g. var a = 2; { a } == { a: a }
        /// </summary>
        public bool IsAlone = false;

        public Property()
        {
            Kind = Kind.Property;
        }
    }
}
