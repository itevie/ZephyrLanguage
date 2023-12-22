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
        public static Package ModulePackage = new Package("Module", new Values.ObjectValue(new
        {
            keys = new NativeFunction((options) =>
            {
                ModuleValue objectValue = (ModuleValue)options.Arguments[0];
                List<RuntimeValue> keys = new List<RuntimeValue>();

                foreach (KeyValuePair<string, RuntimeValue> values in objectValue.Variables)
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
                new Parameter(new VariableType(Values.ValueType.Module), "module")
            })),
        }));
    }
}
