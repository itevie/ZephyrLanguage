using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static NativeFunction CreateNativeFunction(Func<List<RuntimeValue>, Environment, Parser.AST.Expression?, RuntimeValue> function, string name = "unknown", NativeFunctionOptions? options = null)
        {
            return new NativeFunction(function)
            {
                Name = options != null ? options.Name : name,
                Options = options == null ? new NativeFunctionOptions() : options
            };
        }
    }
}
