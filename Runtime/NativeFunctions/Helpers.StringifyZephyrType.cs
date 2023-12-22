using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Helpers
    {
        public static string StringifyZephyrType(RuntimeValue value)
        {
            switch (value.Type.TypeName)
            {
                case Values.ValueType.Number:
                    return $"{((NumberValue)value).Value}";
                case Values.ValueType.String:
                    return $"\"{((StringValue)value).Value}\"";
                case Values.ValueType.Boolean:
                    return $"{((BooleanValue)value).Value.ToString().ToLower()}";
                case Values.ValueType.Object:
                    return StringifyZephyrObject(((ObjectValue)value));
                case Values.ValueType.Null:
                    return "null";
                case Values.ValueType.Array:
                    string stringVal = $"[";

                    int idx = 0;
                    foreach (RuntimeValue val in ((ArrayValue)value).Items)
                    {
                        stringVal += StringifyZephyrType(val);
                        idx++;
                        if (idx != ((ArrayValue)value).Items.Count)
                        {
                            stringVal += ",";
                        }
                    }
                    stringVal += "]";
                    return stringVal;
                default:
                    return "\"?\"";
            }
        }
    }
}
