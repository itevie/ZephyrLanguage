using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer
{
    internal class LexerException : ZephyrException
    {
        public LexerException(ZephyrExceptionOptions options) : base(options) { }
    }
}
