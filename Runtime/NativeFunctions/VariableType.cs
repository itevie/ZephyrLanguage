using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package VariablePkg = new Package("Variable", new
        {
            getType = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(((Values.VariableValue)args[0]).Variable.Value.Type.ToString());
            }, options: new()
            {
                Parameters =
                {
                    new()
                    {
                        Name = "variableRefernce",
                        Type = Values.ValueType.Variable
                    }
                }
            }),

            getName = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(((Values.VariableValue)args[0]).Variable.Name);
            }, options: new()
            {
                Parameters =
                {
                    new()
                    {
                        Name = "variableRefernce",
                        Type = Values.ValueType.Variable
                    }
                }
            }),

            getValue = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return ((Values.VariableValue)args[0]).Variable.Value;
            }, options: new()
            {
                Parameters =
                {
                    new()
                    {
                        Name = "variableRefernce",
                        Type = Values.ValueType.Variable
                    }
                }
            })
        });
    }
}
