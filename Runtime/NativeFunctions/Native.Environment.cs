using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package EnvironmentPackage = new Package("Environment", new ObjectValue(new
        {
            getVariables = new NativeFunction(options =>
            {
                ObjectValue value = new ObjectValue();

                foreach (KeyValuePair<string, Variable> kv in options.Environment.GetVariables())
                {
                    value.Properties.Add(kv.Key, new VariableReference(kv.Value));
                }

                return value;
            }, new Options("getVariables")),
        }));
    }
}
