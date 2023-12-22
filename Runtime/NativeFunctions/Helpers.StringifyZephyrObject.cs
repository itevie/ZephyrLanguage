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
        public static string StringifyZephyrObject(ObjectValue obj)
        {
            string stringVal = "{";

            int cidx = 0;

            foreach (KeyValuePair<string, RuntimeValue> kv in obj.Properties)
            {
                cidx++;
                string name = $"\"{kv.Key}\"";

                switch (kv.Value.Type.TypeName)
                {
                    case Values.ValueType.Number:
                        stringVal += $"{name}:{((NumberValue)kv.Value).Value}";
                        break;
                    case Values.ValueType.String:
                        stringVal += $"{name}:\"{((StringValue)kv.Value).Value}\"";
                        break;
                    case Values.ValueType.Boolean:
                        stringVal += $"{name}:{((BooleanValue)kv.Value).Value.ToString().ToLower()}";
                        break;
                    case Values.ValueType.Object:
                        stringVal += $"{name}:" + StringifyZephyrObject(((ObjectValue)kv.Value));
                        break;
                    case Values.ValueType.Null:
                        stringVal += $"{name}:null";
                        break;
                    case Values.ValueType.Array:
                        stringVal += $"{name}: [";

                        int idx = 0;
                        foreach (RuntimeValue val in ((ArrayValue)kv.Value).Items)
                        {
                            stringVal += StringifyZephyrType(val);
                            idx++;
                            if (idx != ((ArrayValue)kv.Value).Items.Count)
                            {
                                stringVal += ",";
                            }
                        }

                        stringVal += "]";
                        break;
                }

                if (cidx != obj.Properties.Count)
                {
                    stringVal += ",";
                }
            }

            stringVal += "}";

            return stringVal;
        }
    }
}
