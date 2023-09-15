using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class VariableValue : RuntimeValue
    {
        public Variable Variable;

        public VariableValue(Variable variable)
        {
            Type = ValueType.Variable;
            Variable = variable;
        }
    }
}
