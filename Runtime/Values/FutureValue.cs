using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class FutureValue : RuntimeValue
    {
        public Task<RuntimeValue> Task;

        public FutureValue(Task<RuntimeValue> task)
        {
            Task = task;
            Type = new VariableType(Values.ValueType.Future);
        }
    }
}
