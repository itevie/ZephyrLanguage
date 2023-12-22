using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static bool CanHaveGenerics(Values.ValueType type)
        {
            return type == ValueType.Function;
        }
    }
}
