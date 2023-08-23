using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser
{
    internal class RuntimeException : ZephyrException
    {
        public RuntimeException(ZephyrExceptionOptions options) : base(GenerateExceptionText(options))
        {

        }

        public RuntimeException(string msg, bool isOnlyString) : base(msg, isOnlyString)
        {

        }

        public static new ZephyrExceptionOptions GenerateExceptionText(ZephyrExceptionOptions options)
        {
            options.Error = $"Runtime error: {options.Error}";
            return options;
        }
    }
}
