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

        public static Package IntegerTypePkg = new Package("Integer", new
        {
            toString = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(((IntegerValue)args[0]).Value.ToString());
            }, options: new()
            {
                Name = "toString",
                Parameters =
                {
                    new()
                    {
                        Name = "integer",
                        Type = Values.ValueType.Int
                    }
                }
            }),

            toFloat = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateFloat((float)((IntegerValue)args[0]).Value);
            }, options: new()
            {
                Name = "toFloat",
                Parameters =
                {
                    new()
                    {
                        Name = "integer",
                        Type = Values.ValueType.Int
                    }
                }
            }),

            toNegative = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateInteger(-((IntegerValue)args[0]).Value);
            }, options: new()
            {
                Name = "toNegative",
                Parameters =
                {
                    new()
                    {
                        Name = "integer",
                        Type = Values.ValueType.Int
                    }
                }
            }),
        });
    }
}
