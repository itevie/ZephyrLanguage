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
        public static Package Timers = new Package("timers", new
        {
            start = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return GenerateTimerObject();
            }, options: new()
            {
                Name = "start",
            }),
        });

        public static ObjectValue GenerateTimerObject()
        {
            long start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long elapsed = 0;

            return Helpers.CreateObject(new
            {
                end = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    elapsed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - start;
                    return Helpers.CreateString(elapsed.ToString());
                }, options: new()
                {
                    Name = "end",
                }),
            });
        }
    }
}
