using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class StructValue : RuntimeValue
    {
        public Dictionary<string, VariableType> Fields;

        public StructValue(Dictionary<string, VariableType> fields)
        {
            Type = new VariableType(ValueType.Struct);
            Fields = fields;
        }
    }
}
