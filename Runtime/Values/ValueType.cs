using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal enum ValueType
    {
        Null,

        // Numbers
        Int,
        Long,
        Float,
        Double,

        // Internal
        Variable,
        Number,
        NumberNotFloat,

        // Others
        String,
        Boolean,
        Maybe,
        Object,
        Struct,

        // Functions
        NativeFunction,
        Function,

        // Iterables
        Array,
        Enumerable,

        // Special
        Any,
        Auto,
        Event,
    }
}
