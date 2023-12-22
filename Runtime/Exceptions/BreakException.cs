using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime.Exceptions
{
    /// <summary>
    /// This is thrown when a `break` is found.
    /// A loop will detect this, however if there is no loop, it will raise an error.
    /// </summary>
    internal class BreakException : RuntimeException
    {
        public BreakException(Location location)
            : base($"Cannot break here", location)
        {

        }
    }
}
