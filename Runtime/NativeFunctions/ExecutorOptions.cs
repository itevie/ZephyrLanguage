using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal class ExecutorOptions
    {
        public List<RuntimeValue> Arguments;
        public Environment Environment;
        public Location Location;

        public ExecutorOptions(List<RuntimeValue> arguments, Environment environment, Location location)
        {
            Arguments = arguments;
            Environment = environment;
            Location = location;
        }
    }
}
