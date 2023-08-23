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
            switch (value.Type)
            {
                case Values.ValueType.Int:
                    return ((IntegerValue)value).Value > 0;
                case Values.ValueType.Boolean:
                    return ((BooleanValue)value).Value == true;
                case Values.ValueType.String:
                    return ((StringValue)value).Value != "";
                case Values.ValueType.Null:
                    return false;
                case Values.ValueType.NativeFunction:
                    return true;
                default:
                    throw new Exception($"Cannot evaluate truthy or falsy on type {value.Type}");
            }
        }
    }
}
