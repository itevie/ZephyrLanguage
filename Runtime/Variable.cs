using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime
{
    internal class Variable
    {
        public Values.RuntimeValue Value;
        public VariableSettings Options;

        public Variable(Values.RuntimeValue value, VariableSettings? settings = null)
        {
            Value = value;
            Options = settings ?? new VariableSettings();
        }
    }
}
