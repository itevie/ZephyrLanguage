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
        public string Name;

        public Variable(Values.RuntimeValue value, string name = "Unkown!", VariableSettings? settings = null)
        {
            Value = value;
            Options = settings ?? new VariableSettings();
            Name = name;
        }
    }
}
