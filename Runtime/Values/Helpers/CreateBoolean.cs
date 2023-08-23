using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static BooleanValue CreateBoolean(bool value)
        {
            return new BooleanValue()
            {
                Value = value,
            };
        }
    }
}
