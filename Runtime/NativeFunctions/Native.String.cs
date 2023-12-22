using System.ComponentModel.Design;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package StringPackage = new Package("String", new Values.ObjectValue(new
        {
            split = new Values.NativeFunction((options) =>
            {
                // Collect values
                List<RuntimeValue> values = new List<RuntimeValue>();
                List<string> strings = new List<string>();

                string toSplit = ((StringValue)options.Arguments[0]).Value;
                string seperator = ((StringValue)options.Arguments[1]).Value;

                // Check if empty
                if (seperator == "")
                    strings = toSplit.ToCharArray().Select(c => c.ToString()).ToList<string>();
                else strings = toSplit.Split(seperator).ToList();

                // Add strings as string values to the list
                foreach (string str in strings)
                    values.Add(new StringValue(str));

                // Return value
                return new ArrayValue(values, new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayDepth = 1,
                    ArrayType = Values.ValueType.String
                });
            }, new Options("split", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "toSplit"),
                new Parameter(new VariableType(Values.ValueType.String), "seperator"),
            })),

            join = new NativeFunction((executorOptions) =>
            {
                // Collect items
                List<RuntimeValue> array = ((ArrayValue)executorOptions.Arguments[0]).Items;
                string[] strings = array.Select(i => ((StringValue)i).Value).ToArray();

                // Join
                return new StringValue(string.Join(((StringValue)executorOptions.Arguments[1]).Value, strings));
            }, new Options("join", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayDepth = 1,
                    ArrayType = Values.ValueType.String
                }, "strings"),
                new Parameter(new VariableType(Values.ValueType.String), "joinWith")
            })),

            concat = new Values.NativeFunction((executorOptions) =>
            {
                string finishedValue = "";

                foreach (RuntimeValue value in executorOptions.Arguments)
                {
                    finishedValue += ((StringValue)value).Value;
                }

                return new StringValue(finishedValue);
            }, new Options("concat")
            {
                AllOfType = new VariableType(Values.ValueType.String)
            }),

            // ----- Basic string functions -----

            toLower = new NativeFunction((options) =>
            {
                return new StringValue(((StringValue)options.Arguments[0]).Value.ToLower());
            }, new Options("toLower", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
            })), 

            toUpper = new NativeFunction((options) =>
            {
                return new StringValue(((StringValue)options.Arguments[0]).Value.ToUpper());
            }, new Options("toUpper", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
            })),

            contains = new NativeFunction((options) =>
            {
                return new BooleanValue(((StringValue)options.Arguments[0]).Value.Contains(((StringValue)options.Arguments[1]).Value));
            }, new Options("contains", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
                new Parameter(new VariableType(Values.ValueType.String), "str2"),
            })),

            toTitle = new NativeFunction((options) =>
            {
                string toSplit = ((StringValue)options.Arguments[0]).Value;

                if (toSplit.Length == 0)
                    return new StringValue("");

                List<string> strings = toSplit.Split(" ").ToList();
                string newString = string.Join(" ", strings.Select(x =>
                    x[0].ToString().ToUpper() + x[1..]
                ));

                return new StringValue(newString);
            }, new Options("toTitle", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
            })),

            padStart = new NativeFunction((options) =>
            {
                // Collect variables
                string str = ((StringValue)options.Arguments[0]).Value;
                string padWith = ((StringValue)options.Arguments[2]).Value;
                long howMany = (long)((NumberValue)options.Arguments[1]).Value;

                // Check errors
                if (padWith.Length != 1)
                    throw new RuntimeException($"Parameter 3 must be exactly 1 character long", Location.UnknownLocation);
                if (howMany > int.MaxValue)
                    throw new RuntimeException($"Parameter 3 is too large, maximum of {int.MaxValue} is allowed", Location.UnknownLocation);

                return new StringValue(str.PadLeft((int)howMany, padWith.ToCharArray()[0]));
            }, new Options("padStart", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
                new Parameter(new VariableType(Values.ValueType.Number), "howMany"),
                new Parameter(new VariableType(Values.ValueType.String), "with"),
            })),

            padEnd = new NativeFunction((options) =>
            {
                // Collect variables
                string str = ((StringValue)options.Arguments[0]).Value;
                string padWith = ((StringValue)options.Arguments[2]).Value;
                long howMany = (long)((NumberValue)options.Arguments[1]).Value;

                // Check errors
                if (padWith.Length != 1)
                    throw new RuntimeException($"Parameter 3 must be exactly 1 character long", Location.UnknownLocation);
                if (howMany > int.MaxValue)
                    throw new RuntimeException($"Parameter 3 is too large, maximum of {int.MaxValue} is allowed", Location.UnknownLocation);

                return new StringValue(str.PadRight((int)howMany, padWith.ToCharArray()[0]));
            }, new Options("padEnd", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
                new Parameter(new VariableType(Values.ValueType.Number), "howMany"),
                new Parameter(new VariableType(Values.ValueType.String), "with"),
            })),

            charCodeAt = new NativeFunction((options) =>
            {
                string str = ((StringValue)options.Arguments[0]).Value;
                long at = (long)((NumberValue)options.Arguments[1]).Value;

                // Check lengths
                if (str.Length - 1 < at)
                    throw new RuntimeException($"Index out of bounds", Location.UnknownLocation);

                return new NumberValue(str.ToCharArray()[at]);
            }, new Options("padEnd", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str"),
                new Parameter(new VariableType(Values.ValueType.Number), "where"),
            })),
        }));
    }
}
