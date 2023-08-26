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
        public static Package ArrayTypePkg = new("Array", new
        {
            add = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Any });
                Util.CheckRecursive(args[0], args[1]);
                ((ArrayValue)args[0]).Items.Add(args[1]);
                return ((ArrayValue)args[0]);
            }, options: new()
            {
                Name = "add",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "element",
                        Type = Values.ValueType.Any
                    }
                }
            }),


            fill = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Any }, expr);
                Util.CheckRecursive(args[0], args[1]);

                ArrayValue arrValue = (ArrayValue)args[0];

                for (int i = 0; i < arrValue.Items.Count; i++)
                {
                    arrValue.Items[i] = args[1];
                }

                return arrValue;
            }, options: new()
            {
                Name = "fill",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "withWhat",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            at = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Int }, expr);

                ArrayValue arr = (ArrayValue)args[0];
                int idx = (int)((IntegerValue)args[1]).Value;

                // Check idx
                if (idx < 0 || idx > arr.Items.Count - 1)
                    throw new RuntimeException(new()
                    {
                        Location = args[1].Location,
                        Error = "Array index out of bounds"
                    });

                return arr.Items[idx];
            }, options: new()
            {
                Name = "at",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "index",
                        Type = Values.ValueType.Int
                    }
                }
            }),

            set = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Int, Values.ValueType.Any }, expr);
                Util.CheckRecursive(args[0], args[2]);

                ArrayValue arr = (ArrayValue)args[0];
                int idx = (int)((IntegerValue)args[1]).Value;
                RuntimeValue addingValue = args[2];

                // Check idx
                if (idx < 0 || idx > arr.Items.Count - 1)
                    throw new RuntimeException(new()
                    {
                        Location = args[1].Location,
                        Error = "Array index out of bounds"
                    });

                arr.Items[idx] = addingValue;
                return arr;
            }, options: new()
            {
                Name = "set",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "index",
                        Type = Values.ValueType.Int
                    },
                    new()
                    {
                        Name = "element",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            removeAt = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Int }, expr);

                ArrayValue arr = (ArrayValue)args[0];
                int idx = (int)((IntegerValue)args[1]).Value;

                // Check idx
                if (idx < 0 || idx > arr.Items.Count - 1)
                    throw new RuntimeException(new()
                    {
                        Location = args[1].Location,
                        Error = "Array index out of bounds"
                    });

                arr.Items.RemoveAt(idx);
                return arr;
            }, options: new()
            {
                Name = "removeAt",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "index",
                        Type = Values.ValueType.Int
                    }
                }
            }),

            transform = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Function }, expr);

                List<RuntimeValue> newArray = new();
                List<RuntimeValue> oldArray = ((ArrayValue)args[0]).Items;
                FunctionValue function = (FunctionValue)args[1];

                for (int i = 0; i < oldArray.Count; i++)
                {
                    RuntimeValue val = Handlers.Helpers.EvaluateFunctionHelper(function, new List<RuntimeValue>()
                    {
                        oldArray[i],
                        Helpers.CreateInteger(i)
                    }, env, expr);

                    newArray.Add(val);
                }

                return Helpers.CreateArray(newArray);
            }, options: new()
            {
                Name = "transform",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "transformFunction",
                        Type = Values.ValueType.Function
                    }
                }
            }),

            join = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.String }, expr);

                string finished = "";

                foreach (RuntimeValue value in ((ArrayValue)args[0]).Items)
                {
                    // Check type
                    if (value.Type != Values.ValueType.String)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = args[0].Location,
                            Error = "Expected all items in array to be of type string"
                        });
                    }
                }

                finished = string.Join(((StringValue)args[1]).Value, ((ArrayValue)args[0]).Items.Select(x => ((StringValue)x).Value));

                return Helpers.CreateString(finished);
            }, options: new()
            {
                Name = "join",
                Parameters =
                {
                    new()
                    {
                        Name = "array",
                        Type = Values.ValueType.Array
                    },
                    new()
                    {
                        Name = "withWhat",
                        Type = Values.ValueType.String
                    }
                }
            }),
        });
    }
}
