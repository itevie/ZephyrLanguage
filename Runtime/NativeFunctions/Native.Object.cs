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
        public static Package ObjectPackage = new Package("Object", new Values.ObjectValue(new
        {
            keys = new NativeFunction((options) =>
            {
                ObjectValue objectValue = (ObjectValue)options.Arguments[0];
                List<RuntimeValue> keys = new List<RuntimeValue>();

                foreach (KeyValuePair<string, RuntimeValue> values in objectValue.Properties)
                {
                    keys.Add(new StringValue(values.Key));
                }

                return new ArrayValue(keys, new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayDepth = 1,
                    ArrayType = Values.ValueType.String,
                });
            }, new Options("keys", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Object), "obj")
            })),

            forEach = new NativeFunction((options) =>
            {
                ObjectValue obj= (ObjectValue)options.Arguments[0];
                FunctionValue callback = (FunctionValue)options.Arguments[1];

                foreach (KeyValuePair<string, RuntimeValue> value in obj.Properties)
                {
                    Handlers.Helpers.ExecuteZephyrFunction(callback, new List<RuntimeValue>() { new StringValue(value.Key), value.Value }, options.Environment);
                }

                return new NullValue();
            }, new Options("forEach", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Object), "object"),
                new Parameter(new VariableType(Values.ValueType.Function), "fn"),
            })),
        }));
    }
}
