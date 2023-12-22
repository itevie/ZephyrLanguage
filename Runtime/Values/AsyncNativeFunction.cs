using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class AsyncNativeFunction : RuntimeValue
    {
        public Func<NativeFunctions.ExecutorOptions, Task<RuntimeValue>> Call;
        public NativeFunctions.Options Options;

        public AsyncNativeFunction(Func<NativeFunctions.ExecutorOptions, Task<RuntimeValue>> func, NativeFunctions.Options options)
        {
            Type = new VariableType(Values.ValueType.AsyncNativeFunction);
            Call = func;
            Options = options;
        }
    }
}
