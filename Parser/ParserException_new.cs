using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser
{
    internal class ParserException_new : ZephyrException_new
    {
        public ParserException_new() : base()
        {
            ErrorType = Errors.ErrorType.ParserError;
        }
    }
}
