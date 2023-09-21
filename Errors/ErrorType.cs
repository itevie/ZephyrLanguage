using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Errors
{
    internal enum ErrorType
    {
        LexerError,
        ParserError,
        RuntimeError,
        UserDefinedError
    }
}
