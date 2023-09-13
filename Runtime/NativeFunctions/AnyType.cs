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
        public static Package AnyType = new("Any", new
        {
            length = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateNull();
            }),

            type = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(args[0].Type.ToString());
            }, options: new()
            {
                Name = "type",
                Parameters =
                {
                    new()
                    {
                        Name = "theVariable",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            isNull = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateBoolean(args[0].Type == Values.ValueType.Null);
            }, options: new()
            {
                Name = "isNull",
                Parameters =
                {
                    new()
                    {
                        Name = "theVariable",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            hasModifier = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                if (args[0].Modifiers.Contains((Modifier)((IntegerValue)args[1]).Value))
                {
                    return Helpers.CreateBoolean(true);
                } return Helpers.CreateBoolean(false);
            }, options: new()
            {
                Name = "hasModifier",
                Parameters =
                {
                    new()
                    {
                        Name = "theVariable",
                        Type = Values.ValueType.Any
                    },
                    new()
                    {
                        Name = "modifierEnum",
                        Type = Values.ValueType.Int
                    }
                }
            }),
        });
    }
}
