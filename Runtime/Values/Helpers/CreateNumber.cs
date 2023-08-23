using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static NumberValue_deprecated CreateNumber(float value)
        {
            Console.WriteLine("Number created!");
            Console.WriteLine(new StackFrame(1, true));
            return new NumberValue_deprecated()
            {
                Value = value,
            };
        }
    }
}
