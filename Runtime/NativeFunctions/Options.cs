using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal class Options
    {
        public string Name;
        public string? Description { get; set; } = null;
        public List<Parameter> Parameters = new List<Parameter>();
        public bool IsManaged { get; set; } = false;
        public VariableType? AllOfType { get; set; } = null;

        public Options(string name, List<Parameter>? parameters = null)
        {
            Name = name;
            if (parameters != null)
                Parameters = parameters;
        }
    }
}
