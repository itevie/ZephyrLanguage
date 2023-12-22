using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static bool IsValueTruthy(RuntimeValue value)
        {
            return value.Type.TypeName switch
            {
                ValueType.Boolean => ((BooleanValue)value).Value == true,
                ValueType.String => ((StringValue)value).Value.Length != 0,
                ValueType.Number => ((NumberValue)value).Value > 0,
                ValueType.Null => false,
                ValueType.Object => true,
                ValueType.Array => true,
                _ => throw new RuntimeException($"Cannot check if {Helpers.VisualiseType(value.Type)} is truthy", Location.UnknownLocation)
            };
        }
    }
}
