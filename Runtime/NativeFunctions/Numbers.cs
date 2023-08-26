using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package Number = new("numbers", new
        {
            negative = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Int }, expr);
                return Helpers.CreateNumber(-Math.Abs(((IntegerValue)args[0]).Value));
            }, "negative")
        });
    }
}
