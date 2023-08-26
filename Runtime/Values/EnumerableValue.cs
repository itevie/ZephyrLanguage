using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class EnumerableValue : RuntimeValue
    {
        public List<RuntimeValue> Values = new();

        public EnumerableValue()
        {
            Type = ValueType.Enumerable;
        }
    }
}
