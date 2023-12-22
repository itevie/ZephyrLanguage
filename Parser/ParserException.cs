using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser
{
    internal class ParserException : ZephyrException
    {
        public ParserException(string message, Location location) : base(message, location)
        {
            ErrorType = Errors.ErrorType.Parser;
        }
    }
}
