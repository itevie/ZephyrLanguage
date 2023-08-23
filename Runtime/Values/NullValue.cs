using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class NullValue : RuntimeValue
    {
        public object? Value = null;
        
        public NullValue()
        {
            Type = ValueType.Null;
        }
    }
}
