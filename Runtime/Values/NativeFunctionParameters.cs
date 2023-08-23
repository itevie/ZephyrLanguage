using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class NativeFunctionParameter
    {
        public ValueType Type { get; set; } = ValueType.Any;
        public string? Name { get; set; } = null;
    }
}
