using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static void CanAddToArray(ArrayValue array, RuntimeValue value)
        {
            VariableType type;
            if (array.Type.ArrayDepth == 1)
            {
                type = new VariableType(array.Type.ArrayType);
            } else
            {
                type = new VariableType(Values.ValueType.Array)
                {
                    IsArray = true,
                    ArrayDepth = array.Type.ArrayDepth - 1,
                    ArrayType = array.Type.ArrayType
                };
            }

            Values.Helpers.TypeMatches(type, value);
        }
    }
}
