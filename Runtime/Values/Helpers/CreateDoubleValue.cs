using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static DoubleValue CreateDoubleValue(double value)
        {
            return new DoubleValue()
            {
                Value = value,
            };
        }
    }
}
