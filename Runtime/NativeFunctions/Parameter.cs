using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal class Parameter
    {
        public VariableType Type;
        public string? Name;

        public Parameter(VariableType type, string? name)
        {
            Type = type;
            Name = name;
        }
    }
}
