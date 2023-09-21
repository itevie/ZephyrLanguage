using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer
{
    internal class LexerException_new : ZephyrException_new
    {
        public LexerException_new()
        {
            ErrorType = Errors.ErrorType.LexerError;
        }
    }
}
