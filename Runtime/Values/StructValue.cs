using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class StructValue : ObjectValue
    {
        public StructValue()
        {
            Type = ValueType.Struct;
        }
    }
}
