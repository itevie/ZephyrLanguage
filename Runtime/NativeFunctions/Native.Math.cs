using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package MathPackage = new Package("Math", new Values.ObjectValue(new
        {
            pi = Math.PI,
            e = Math.E,

            // ----- Basic functions -----

            abs = new NativeFunction((options) =>
            {
                double originalValue = ((NumberValue)options.Arguments[0]).Value;
                double newValue = Math.Abs(originalValue);

                return new NumberValue(newValue);
            }, new Options("abs", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
            })),

            truncate = new NativeFunction((options) =>
            {
                double originalValue = ((NumberValue)options.Arguments[0]).Value;

                return new NumberValue(Math.Truncate(originalValue));
            }, new Options("truncate", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
            })),

            ceil = new NativeFunction((options) =>
            {
                double originalValue = ((NumberValue)options.Arguments[0]).Value;

                return new NumberValue(Math.Ceiling(originalValue));
            }, new Options("ceil", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
            })),

            floor = new NativeFunction((options) =>
            {
                double originalValue = ((NumberValue)options.Arguments[0]).Value;

                return new NumberValue(Math.Floor(originalValue));
            }, new Options("floor", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
            })),

            round = new NativeFunction((options) =>
            {
                double originalValue = ((NumberValue)options.Arguments[0]).Value;

                return new NumberValue(Math.Round(originalValue));
            }, new Options("round", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
            })),

            // ----- More complex functions -----

            clamp = new NativeFunction((options) =>
            {
                double value = ((NumberValue)options.Arguments[0]).Value;
                double min = ((NumberValue)options.Arguments[1]).Value;
                double max = ((NumberValue)options.Arguments[2]).Value;

                return new NumberValue(Math.Min(Math.Max(value, min), max));
            }, new Options("clamp", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "number"),
                new Parameter(new VariableType(Values.ValueType.Number), "min"),
                new Parameter(new VariableType(Values.ValueType.Number), "max"),
            })),

            min = new NativeFunction((options) =>
            {
                if (options.Arguments.Count == 0)
                    throw new RuntimeException($"Expected at least one parameger", Location.UnknownLocation);
                double? min = null;

                foreach (RuntimeValue val in options.Arguments)
                {
                    double value = ((NumberValue)val).Value;

                    if (min == null)
                        min = value;
                    else if (min > value)
                        min = value;
                }

                return new NumberValue(min ?? 0);
            }, new Options("min")
            {
                AllOfType = new VariableType(Values.ValueType.Number)
            }),

            max = new NativeFunction((options) =>
            {
                if (options.Arguments.Count == 0)
                    throw new RuntimeException($"Expected at least one parameger", Location.UnknownLocation);
                double? max = null;

                foreach (RuntimeValue val in options.Arguments)
                {
                    double value = ((NumberValue)val).Value;

                    if (max == null)
                        max = value;
                    else if (max < value)
                        max = value;
                }

                return new NumberValue(max ?? 0);
            }, new Options("max")
            {
                AllOfType = new VariableType(Values.ValueType.Number)
            }),

            // ----- Random -----

            randomFrom = new NativeFunction((options) =>
            {
                double minimum = ((NumberValue)options.Arguments[0]).Value;
                double maximum = ((NumberValue)options.Arguments[1]).Value;

                Random random = new Random();
                return new NumberValue(random.NextDouble() * (maximum - minimum) + minimum);
            }, new Options("randomFrom", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "min"),
                new Parameter(new VariableType(Values.ValueType.Number), "max"),
            })),

            randomIntegerFrom = new NativeFunction((options) =>
            {
                long minimum = (long)((NumberValue)options.Arguments[0]).Value;
                long maximum = (long)((NumberValue)options.Arguments[1]).Value;

                Random random = new Random();
                return new NumberValue((long)(random.NextDouble() * (maximum - minimum) + minimum));
            }, new Options("randomIntegerFrom", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "min"),
                new Parameter(new VariableType(Values.ValueType.Number), "max"),
            })),

            random = new NativeFunction((options) =>
            {
                Random random = new Random();
                return new NumberValue(random.NextDouble());
            }, new Options("random")),

            randomBool = new NativeFunction((options) =>
            {
                Random random = new Random();
                return new BooleanValue(random.NextDouble() > 0.5);
            }, new Options("random")),

            randomInteger = new NativeFunction((options) =>
            {
                Random random = new Random();
                return new NumberValue(random.Next());
            }, new Options("randomInteger")),

            randomItem = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];

                Random random = new Random();
                int where = random.Next(array.Items.Count);
                return array.Items[where];
            }, new Options("randomItem", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array")
            }))
        }));
    }
}

