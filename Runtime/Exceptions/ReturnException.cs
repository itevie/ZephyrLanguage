using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Exceptions
{
    /// <summary>
    /// This is thrown when a `return` is found.
    /// A function call will detect this, however if there is no function, it will raise an error.
    /// </summary>
    internal class ReturnException : RuntimeException
    {
        public RuntimeValue Value;

        public ReturnException(Location location, RuntimeValue value)
            : base($"Cannot return here", location)
        {
            Value = value;
        }
    }
}
