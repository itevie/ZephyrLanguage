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
        public static Package Arrays = new Package("arrays", new
        {
            fill = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ArrayValue arrValue = (ArrayValue)args[0];

                for (int i = 0; i < arrValue.Items.Count; i++)
                {
                    arrValue.Items[i] = args[1];
                }

                return arrValue;
            }, options: new()
            {
                Name = "split",
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

            add = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Any }, expr);

                ArrayValue arr = (ArrayValue)args[0];
                arr.Items.Add(args[1]);
                return arr;
            }, "add"),

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
            }, "at"),

            set = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Int, Values.ValueType.Any }, expr);

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
            }, "set"),

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
            }, "removeAt"),

            transform = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Array, Values.ValueType.Function }, expr);

                List<RuntimeValue> newArray = new List<RuntimeValue>();
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
            }, "transform")
        });
    }
}
