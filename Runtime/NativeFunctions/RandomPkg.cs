﻿using System;
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

        public static Package RandomPkg = new Package("random", new
        {
            value = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Random random = new Random();
                return ((ArrayValue)args[0]).Items[random.Next(0, ((ArrayValue)args[0]).Items.Count)];
            }, options: new()
            {
                Name = "value",
                Parameters =
                {
                    new()
                    {
                        Name = "arr",
                        Type = Values.ValueType.Array
                    },
                }
            }),
        });
    }
}