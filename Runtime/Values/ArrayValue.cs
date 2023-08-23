using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class ArrayValue : RuntimeValue
    {
        public List<RuntimeValue> Items { get; set; } = new List<RuntimeValue>();
        public ValueType ItemsType { get; set; } = ValueType.Array;

        public ArrayValue()
        {
            Type = ValueType.Array;
        }
    }
}
