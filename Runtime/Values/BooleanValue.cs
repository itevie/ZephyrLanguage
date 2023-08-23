using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class BooleanValue : RuntimeValue
    {
        public bool Value = false;

        public BooleanValue()
        {
            Type = ValueType.Boolean;
        }
    }
}
