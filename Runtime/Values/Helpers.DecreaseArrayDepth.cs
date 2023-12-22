using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static VariableType DecreaseArrayDepth(VariableType old)
        {
            if (old.ArrayDepth == 1)
            {
                old.ArrayDepth = 0;
                old.IsArray = false;
                old.TypeName = old.ArrayType;
                return old;
            }

            old.ArrayDepth = old.ArrayDepth - 1;
            return old;
        }
    }
}
