using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime
{
    internal class RuntimeException : ZephyrException
    {
        public RuntimeException(string message, Location location) : base(message, location)
        {
            ErrorType = Errors.ErrorType.Runtime;
        }
    }
}
