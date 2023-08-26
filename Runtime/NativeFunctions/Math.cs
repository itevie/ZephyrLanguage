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
        public static Package MathPkg = new("Math", new
        {
            abs = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateInteger(1);
            }, options: new()
            {
                Name = "abs",
                Parameters =
                {
                    new()
                    {
                        Name = "theNumber",
                        Type = Values.ValueType.Number
                    }
                }
            })
        });
    }
}
