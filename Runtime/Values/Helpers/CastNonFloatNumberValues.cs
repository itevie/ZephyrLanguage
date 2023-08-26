using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        // TODO Don't use dynamic here
        public static RuntimeValue CastNonFloatNumberValues(RuntimeValue value, ValueType newValue)
        {
            RuntimeValue val = new();
            if (newValue is ValueType.Int)
            {
                val = CreateInteger((int)((dynamic)value).Value);
            } else if (newValue == ValueType.Long)
            {
                val = CreateLongValue((long)((dynamic)value).Value);
            } else if (newValue == ValueType.Any)
            {
                if (value.Type != ValueType.Number)
                    val = value;
                else val = CastNonFloatNumberValues(value, ValueType.Int);
            }

            val.Location = value.Location;
            val.IsReturn = value.IsReturn;

            return val;
        }
    }
}
