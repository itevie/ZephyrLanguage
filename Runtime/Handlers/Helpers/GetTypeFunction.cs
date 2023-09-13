using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        /// <summary>
        /// This checks all the in-built type functions.
        /// E.g., Any package, this is avaiable on all types
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="func">The function name</param>
        /// <param name="env">The environemnt to check in</param>
        /// <param name="loc">The location</param>
        /// <returns>A modified native function that was found</returns>
        /// <exception cref="RuntimeException"></exception>
        public static NativeFunction GetTypeFunction(string type, string func, Environment env, Location? loc)
        {
            // Get the type packages
            ObjectValue anyPkg = (ObjectValue)env.LookupVariable("Any");
            ObjectValue givenPkg = (ObjectValue)env.LookupVariable(type);

            NativeFunction givenFunc;

            // Check if the actual type contains the given function
            if (givenPkg.Properties.ContainsKey(func) && ((NativeFunction)givenPkg.Properties[func]).Options.UsableAsTypeFunction)
            {
                givenFunc = (NativeFunction)givenPkg.Properties[func];
            } 
            
            // Check if the any type contains the given function
            else if (anyPkg.Properties.ContainsKey(func) && ((NativeFunction)anyPkg.Properties[func]).Options.UsableAsTypeFunction)
            {
                givenFunc = (NativeFunction)anyPkg.Properties[func];
            } 
            
            // No type function was found
            else
            {
                throw new RuntimeException(new()
                {
                    Location = loc,
                    Error = $"{type} type does not contain this"
                });
            }

            // Re-construct the native function
            NativeFunction f = Values.Helpers.Helpers.CreateNativeFunction(givenFunc.Call);
            f.Name = givenFunc.Name;
            f.Options = givenFunc.Options;
            f.IsTypeCall = true;

            return f;
        }
    }
}
