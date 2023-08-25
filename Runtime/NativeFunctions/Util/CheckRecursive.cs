using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Util
    {
        public static bool CheckRecursive(RuntimeValue initValue, RuntimeValue compareValue)
        {
            Location? location = Handlers.Helpers.GetLocation(compareValue.Location, initValue.Location);

            // Check if outright same
            bool isRecursive = false;
            if (initValue == compareValue) isRecursive = true;

            // Check array
            if (compareValue.Type == Values.ValueType.Array)
            {
                foreach (RuntimeValue val in ((ArrayValue)compareValue).Items)
                {
                    CheckRecursive(initValue, val);
                }
            }

            if (isRecursive)
            {
                throw new RuntimeException(new()
                {
                    Location = location,
                    Error = $"Found recursion"
                });
            }

            return isRecursive;
        }
    }
}
