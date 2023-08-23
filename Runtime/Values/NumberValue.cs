using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class NumberValue_deprecated : RuntimeValue
    {
        public float Value = 0;

        public NumberValue_deprecated()
        {
            Type = ValueType.Number;
        }
    }
}
