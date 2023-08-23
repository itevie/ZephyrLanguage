using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Values
{
    internal class AnyValue : RuntimeValue
    {
        public object? Value { get; set; } = null;

        public AnyValue()
        {
            Type = ValueType.Any;
        }
    }
}
