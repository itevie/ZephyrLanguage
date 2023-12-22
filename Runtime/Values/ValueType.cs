using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal enum ValueType
    {
        Null,

        Number,

        // ----- Functions -----
        Function,
        NativeFunction,
        AsyncNativeFunction,

        // ----- Other -----
        Boolean,
        String,
        Array,
        Object,
        Struct,

        // ----- Special -----
        Any,
        Auto,
        Void,
        VariableReference,
        Module,
        Future,
    }
}
