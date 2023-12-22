using Newtonsoft.Json.Linq;
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
        public static ObjectValue JObjectToZephyrObj(JObject jobj)
        {
            ObjectValue obj = new();

            foreach (JProperty prop in jobj.Properties())
            {
                JTokenType? type = prop.Value?.Type;

                if (type == null || prop.Value == null)
                {
                    obj.Properties.Add(prop.Name, new NullValue());
                    break;
                }

                switch (type)
                {
                    case JTokenType.Integer:
                        obj.Properties.Add(prop.Name, new NumberValue((double)prop.Value));
                        break;
                    case JTokenType.String:
                        obj.Properties.Add(prop.Name, new StringValue(((string?)prop.Value) ?? ""));
                        break;
                    case JTokenType.Object:
                        obj.Properties.Add(prop.Name, JObjectToZephyrObj((JObject)prop.Value));
                        break;
                    case JTokenType.Boolean:
                        obj.Properties.Add(prop.Name, new BooleanValue((bool)prop.Value));
                        break;
                    case JTokenType.Null:
                        obj.Properties.Add(prop.Name, new NullValue());
                        break;
                }
            }

            return obj;
        }
    }
}
