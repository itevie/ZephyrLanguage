using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser
{
    internal class ParserException : ZephyrException
    {
        public ParserException(ZephyrExceptionOptions options) : base(GenerateExceptionText(options))
        {

        }

        public static new ZephyrExceptionOptions GenerateExceptionText(ZephyrExceptionOptions options)
        {
            options.Error = $"Parser error: {options.Error}";
            return options;
        }
    }
}
