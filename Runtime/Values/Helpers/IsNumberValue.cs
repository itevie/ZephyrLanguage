using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static bool IsNumberValue(ValueType type)
        {
            if (type != ValueType.Int && type != ValueType.Double && 
                type != ValueType.Float && type != ValueType.Long &&
                type != ValueType.Number)
                return false;
            return true;
        }
    }
}
