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
        public static Package UtilPkg = new("util", new
        {
            isTruthy = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Any });
                return Helpers.CreateBoolean(Handlers.Helpers.EvaluateTruhyValueHelper(args[0]));
            }, "isTruthy")
        });
    }
}
