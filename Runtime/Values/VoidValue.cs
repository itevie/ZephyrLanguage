using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class VoidValue : RuntimeValue
    {
        public VoidValue()
        {
            Type = new VariableType(Values.ValueType.Void);
        }
    }
}
