using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package ArrayPackage = new Package("Array", new Values.ObjectValue(new
        {
            any = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (RuntimeValue value in array.Items)
                {
                    RuntimeValue result = Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { value }, options.Environment);

                    // Check if boolean
                    if (result.Type.TypeName != Values.ValueType.Boolean)
                        throw new RuntimeException($"Expected a boolean return type", result.Location);

                    if (((BooleanValue)result).Value == true)
                        return new BooleanValue(true);
                }

                return new BooleanValue(false);
            }, new Options("any", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),

            all = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (RuntimeValue value in array.Items)
                {
                    RuntimeValue result = Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { value }, options.Environment);

                    // Check if boolean
                    if (result.Type.TypeName != Values.ValueType.Boolean)
                        throw new RuntimeException($"Expected a boolean return type", result.Location);

                    if (((BooleanValue)result).Value != true)
                        return new BooleanValue(false);
                }

                return new BooleanValue(true);
            }, new Options("all", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),

            select = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];
                ArrayValue newArray = new ArrayValue(new List<RuntimeValue>(), new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayType = Values.ValueType.Any
                });
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (RuntimeValue value in array.Items)
                {
                    RuntimeValue result = Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { value }, options.Environment);

                    // Check if boolean
                    if (result.Type.TypeName != Values.ValueType.Boolean)
                        throw new RuntimeException($"Expected a boolean return type", result.Location);

                    if (((BooleanValue)result).Value == true)
                        newArray.Items.Add(value);
                }

                return newArray;
            }, new Options("select", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),

            map = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];
                ArrayValue newArray = new ArrayValue(new List<RuntimeValue>(), new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayType = Values.ValueType.Any
                });
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (RuntimeValue value in array.Items)
                {
                    RuntimeValue result = Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { value }, options.Environment);
                    newArray.Items.Add(result);
                }

                return newArray;
            }, new Options("map", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),

            forEach = new NativeFunction((options) =>
            {
                ArrayValue array = (ArrayValue)options.Arguments[0];
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (RuntimeValue value in array.Items)
                {
                    Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { value }, options.Environment);
                }

                return new NullValue();
            }, new Options("forEach", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Array), "array"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),
        }));
    }
}
