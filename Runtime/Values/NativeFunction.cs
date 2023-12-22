using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class NativeFunction : RuntimeValue
    {
        public Func<NativeFunctions.ExecutorOptions, RuntimeValue> Call;
        public NativeFunctions.Options Options;

        public NativeFunction(Func<NativeFunctions.ExecutorOptions, RuntimeValue> func, NativeFunctions.Options options)
        {
            Type = new VariableType(Values.ValueType.NativeFunction);
            Call = func;
            Options = options;
        }
    }
}
