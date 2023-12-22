using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static bool CompareTypes(VariableType a, VariableType b)
        {
            if (a.TypeName == ValueType.Any || b.TypeName == ValueType.Any)
                return true;

            // Check direct types
            if (a.TypeName != b.TypeName)
                return false;

            // Check if it is an array
            if (a.IsArray != b.IsArray)
                return false;
            else if (a.IsArray)
            {
                // Check array type
                if (a.ArrayType != b.ArrayType)
                    return false;

                // Check array depth
                if (a.ArrayDepth != b.ArrayDepth) return false;

                // They are the same
                return true;
            }

            return true;
        }
    }
}
