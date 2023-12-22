using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime
{
    internal class Variable
    {
        private RuntimeValue _value;

        public RuntimeValue Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public VariableSettings Settings { get; set; }

        public Variable(RuntimeValue value, VariableSettings settings)
        {
            _value = value;
            Settings = settings;
        }
    }
}
