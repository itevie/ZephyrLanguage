using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;

namespace Zephyr.Runtime.Values
{
    internal class RuntimeValue
    {
        public ValueType Type { get; set; } = ValueType.Any;

        public bool IsReturn = false;
        public Location? Location { get; set; } = null;
        public List<Modifier> Modifiers { get; set; } = new();

        public bool IsFinal()
        {
            return Modifiers.Contains(Modifier.Final);
        }
    }
}
