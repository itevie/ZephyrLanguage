using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        /// <summary>
        /// Allows access to the current scope.
        /// </summary>
        public static Package ScopePackage = new Package("_scope", new
        {
            /*getVariable = Helpers.CreateNativeFunction((args, env, expr) =>
            {

            }, options: new()
            {
                Name = "getVariable",
                Parameters =
                {
                    new()
                    {
                        Name = "variableName",
                        Type = Values.ValueType.String
                    }
                }
            }),*/

            // Returns the current scope's variables as an object, contains ONLY the variables in the scope, not parents'
            getObject = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ObjectValue obj = new();
                
                foreach (KeyValuePair<string, Variable> pair in env._variables)
                {
                    obj.Properties.Add(pair.Key, pair.Value.Value);
                }   

                return obj;
            }, options: new()
            {
                Name = "getVariable"
            })
        });
    }
}
