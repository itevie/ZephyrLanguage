using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Errors
{
    internal enum ErrorType
    {
        Generic,
        Lexer,
        Parser,
        Runtime,
        UserDefined,
    }
}
