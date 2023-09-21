using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static bool EvaluateTruhyValueHelper(RuntimeValue value)
        {
            return value.Type switch
            {
                Values.ValueType.Int => ((IntegerValue)value).Value > 0,
                Values.ValueType.Long => ((LongValue)value).Value > 0,
                Values.ValueType.Float => ((FloatValue)value).Value > 0,
                Values.ValueType.Boolean => ((BooleanValue)value).Value == true,
                Values.ValueType.String => ((StringValue)value).Value != "",
                Values.ValueType.Null => false,
                Values.ValueType.NativeFunction => true,
                _ => throw new RuntimeException_new()
                {
                    Location = value.Location,
                    Error = $"Cannot execute truthy-falsy converter on type {value}"
                }
            };
        }
    }
}
