using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;
using Newtonsoft.Json;
using Zephyr.Parser;
using Newtonsoft.Json.Linq;
using CommandLine;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package JsonPKG = new Package("Json", new
        {
            parse = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                try
                {
                    JObject? parsed = JObject.Parse(((StringValue)args[0]).Value) ?? throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(args[0].Location, expr?.Location),
                            Error = $"Failed to parse JSON: Parser returned null"
                        });

                    return JObjectToZephyrObj(parsed);
                }
                catch (Exception e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(args[0].Location, expr?.Location),
                        Error = $"Failed to parse JSON: {e.Message}"
                    });
                }
            }, options: new()
            {
                Name = "parse",
                Parameters =
                {
                    new()
                    {
                        Name = "jsonString",
                        Type = Values.ValueType.String
                    }
                }
            }),

            toString = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(StringifyZephyrObject((ObjectValue)args[0]));
            }, options: new()
            {
                Name = "toString",
                Parameters =
                {
                    new()
                    {
                        Name = "object",
                        Type = Values.ValueType.Object
                    }
                }
            }),
        });

        public static string StringifyZephyrObject(ObjectValue obj)
        {
            string stringVal = "{";

            int cidx = 0;

            foreach (KeyValuePair<string, RuntimeValue> kv in obj.Properties)
            {
                cidx++;
                string name = $"\"{kv.Key}\"";

                switch (kv.Value.Type)
                {
                    case Values.ValueType.Int:
                        stringVal += $"{name}:{((IntegerValue)kv.Value).Value}";
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
                }

                if (cidx != obj.Properties.Count)
                {
                    stringVal += ",";
                }
            }

            stringVal += "}";

            return stringVal;
        }

        public static string SafifyString(string old)
        {
            return old.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
        }

        public static ObjectValue JObjectToZephyrObj(JObject jobj)
        {
            ObjectValue obj = new ObjectValue();

            foreach (JProperty prop in jobj.Properties())
            {
                JTokenType? type = prop.Value?.Type;

                if (type == null || prop.Value == null)
                {
                    obj.Properties.Add(prop.Name, Helpers.CreateNull());
                    break;
                }

                switch (type)
                {
                    case JTokenType.Integer:
                        obj.Properties.Add(prop.Name, Helpers.CreateInteger((int)prop.Value));
                        break;
                    case JTokenType.String:
                        obj.Properties.Add(prop.Name, Helpers.CreateString(((string?)prop.Value) ?? ""));
                        break;
                    case JTokenType.Object:
                        obj.Properties.Add(prop.Name, JObjectToZephyrObj((JObject)prop.Value));
                        break;
                    case JTokenType.Boolean:
                        obj.Properties.Add(prop.Name, Helpers.CreateBoolean((bool)prop.Value));
                        break;
                    case JTokenType.Null:
                        obj.Properties.Add(prop.Name, Helpers.CreateNull());
                        break;
                }
            }

            return obj;
        }
    }
}
