using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class NativeFunction : RuntimeValue
    {
        public Func<List<RuntimeValue>, Environment, Parser.AST.Expression?, RuntimeValue> Call;
        public string Name = "unknown";
        public bool IsTypeCall { get; set; } = false;
        public RuntimeValue TypeCallValue { get; set; } = new RuntimeValue();
        public NativeFunctionOptions Options { get; set; } = new NativeFunctionOptions();

        public NativeFunction(Func<List<RuntimeValue>, Environment, Parser.AST.Expression?, RuntimeValue> call)
        {
            Type = ValueType.NativeFunction;
            Call = call;
        }
    }
}
