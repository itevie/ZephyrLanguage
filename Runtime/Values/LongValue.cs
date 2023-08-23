using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class LongValue : RuntimeValue
    {
        public long Value = 0;

        public LongValue()
        {
            Type = ValueType.Long;
        }
    }
}
