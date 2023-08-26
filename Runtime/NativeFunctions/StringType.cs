using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package StringTypePkg = new("String", new
        {
            endsWith = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                if (((StringValue)args[0]).Value.EndsWith(((StringValue)args[1]).Value))
                {
                    return Helpers.CreateBoolean(true);
                }
                return Helpers.CreateBoolean(false);
            }, options: new()
            {
                Name = "endsWith",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "what",
                        Type = Values.ValueType.String
                    }
                }
            }),

            startsWith = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                if (((StringValue)args[0]).Value.StartsWith(((StringValue)args[1]).Value))
                {
                    return Helpers.CreateBoolean(true);
                }
                return Helpers.CreateBoolean(false);
            }, options: new()
            {
                Name = "startsWith",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "what",
                        Type = Values.ValueType.String
                    }
                }
            }),

            replace = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string before = ((StringValue)args[0]).Value;
                string what = ((StringValue)args[1]).Value;
                string with = ((StringValue)args[2]).Value;
                return Helpers.CreateString(before.Replace(what, with));
            }, options: new()
            {
                Name = "replace",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "what",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "with",
                        Type = Values.ValueType.String
                    }
                }
            }),

            escape = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string old = ((StringValue)args[0]).Value;
                return Helpers.CreateString(SafifyString(old));
            }, options: new()
            {
                Name = "escape",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                }
            }),

            toLower = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string old = ((StringValue)args[0]).Value;
                return Helpers.CreateString(old.ToLower());
            }, options: new()
            {
                Name = "toLower",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                }
            }),

            toUpper = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string old = ((StringValue)args[0]).Value;
                return Helpers.CreateString(old.ToUpper());
            }, options: new()
            {
                Name = "toUpper",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                }
            }),


            repeat = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string old = ((StringValue)args[0]).Value;
                int amount = ((IntegerValue)args[1]).Value;
                return Helpers.CreateString(string.Concat(System.Linq.Enumerable.Repeat(old, (int)amount)));
            }, options: new()
            {
                Name = "repeat",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "amount",
                        Type = Values.ValueType.Int
                    }
                }
            }),

            toCharCode = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateInteger((int)(((StringValue)args[0]).Value.ToCharArray()[0]));
            }, options: new()
            {
                Name = "toCharCode",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.String
                    },
                }
            }),

            fromCharCode = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(((char)((IntegerValue)args[0]).Value).ToString());
            }, options: new()
            {
                Name = "fromCharCode",
                UsableAsTypeFunction = false,
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.Int
                    },
                }
            }),

            split = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string l = ((StringValue)args[0]).Value;
                string r = ((StringValue)args[1]).Value;

                string[] arr = l.Split(new string[] { r }, StringSplitOptions.None);
                if (r == "") arr = l.ToCharArray().Select(c => c.ToString()).ToArray();

                List<RuntimeValue> runtimeValues = new();

                for (int i = 0; i < arr.Length; i++)
                {
                    runtimeValues.Add(Helpers.CreateString(arr[i]));
                }

                return Helpers.CreateArray(runtimeValues, Values.ValueType.String);
            }, options: new()
            {
                Name = "split",
                Parameters =
                {
                    new()
                    {
                        Name = "stringToSplit",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "with",
                        Type = Values.ValueType.String
                    }
                }
            }),
        });
    }
}
