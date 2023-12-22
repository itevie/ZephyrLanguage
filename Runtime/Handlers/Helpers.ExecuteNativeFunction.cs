using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue ExecuteNativeFunction(NativeFunction function, List<RuntimeValue> arguments, Environment environment, Location? location = null)
        {
            Helpers.ValidateNativeOptions(arguments, function.Options, location ?? Location.UnknownLocation);
            return function.Call(new NativeFunctions.ExecutorOptions(arguments, environment, location ?? Location.UnknownLocation));
        }
        public static RuntimeValue ExecuteNativeFunction(AsyncNativeFunction function, List<RuntimeValue> arguments, Environment environment, Location? location = null)
        {
            Helpers.ValidateNativeOptions(arguments, function.Options, location ?? Location.UnknownLocation);
            return new FutureValue(function.Call(new NativeFunctions.ExecutorOptions(arguments, environment, location ?? Location.UnknownLocation)));
        }
    }
}
