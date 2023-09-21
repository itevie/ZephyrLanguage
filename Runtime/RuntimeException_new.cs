using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime
{
    internal class RuntimeException_new : ZephyrException_new
    {
        public RuntimeException_new()
        {
            ErrorType = Errors.ErrorType.RuntimeError;
        }
    }
}
