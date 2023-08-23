using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static FloatValue CreateFloat(float value)
        {
            return new FloatValue()
            {
                Value = value,
            };
        }
    }
}
