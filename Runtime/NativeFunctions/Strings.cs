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
        public static Package Strings = new Package("strings", new
        {
            split = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.String, Values.ValueType.String }, expr);
                string l = ((StringValue)args[0]).Value;
                string r = ((StringValue)args[1]).Value;

                string[] arr = l.Split(new string[] { r }, StringSplitOptions.None);
                if (r == "") arr = l.ToCharArray().Select(c => c.ToString()).ToArray();

                List<RuntimeValue> runtimeValues = new List<RuntimeValue>();

                for (int i = 0; i < arr.Length; i++)
                {
                    runtimeValues.Add(Helpers.CreateString(arr[i]));
                }

                return Helpers.CreateArray(runtimeValues, Values.ValueType.String);
            }, "split"),

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
            }, "join"),

            toLower = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.String }, expr);

                return Helpers.CreateString(((StringValue)args[0]).Value.ToLower());
            }, "toLower"),

            toUpper = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.String }, expr);

                return Helpers.CreateString(((StringValue)args[0]).Value.ToUpper());
            }, "toUpper"),
            fromCharCode = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(((char)((IntegerValue)args[0]).Value).ToString());
            }, options: new()
            {
                Name = "fromCharCode",
                Parameters =
                {
                    new()
                    {
                        Name = "string",
                        Type = Values.ValueType.Int
                    },
                }
            }),

            toCharCode = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.String }, expr);

                return Helpers.CreateInteger((int)(((StringValue)args[0]).Value.ToCharArray()[0]));
            }),
        });
    }
}
