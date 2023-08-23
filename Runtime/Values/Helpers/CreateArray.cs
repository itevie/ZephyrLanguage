using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static ArrayValue CreateArray(List<RuntimeValue> items, ValueType enforcedType = ValueType.Any)
        {
            return new ArrayValue()
            {
                Items = items,
                ItemsType = enforcedType
            };
        }
    }
}
