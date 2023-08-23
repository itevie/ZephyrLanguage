using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class NativeFunctionOptions
    {
        public string PermissionsNeeded { get; set; } = "";
        public string Name = "unknown";
        public List<NativeFunctionParameter> Parameters { get; set; } = new List<NativeFunctionParameter>();

        public bool UncheckedParameters { get; set; } = false;
        public ValueType? AllParamsOfType { get; set; } = null;
        public bool UsableAsTypeFunction { get; set; } = true;
    }
}
