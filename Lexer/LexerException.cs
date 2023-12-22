using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer
{
    internal class LexerException : ZephyrException
    {
        public LexerException(string message, Location location) : base(message, location)
        {
            ErrorType = Errors.ErrorType.Lexer;
        }
    }
}
