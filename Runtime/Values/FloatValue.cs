using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class FloatValue : RuntimeValue
    {
        public double Value = 0;

        public FloatValue()
        {
            Type = ValueType.Float;
        }
    }
}
