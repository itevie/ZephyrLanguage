using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package TimePkg = new Package("Time", new Values.ObjectValue(new
        {
            units = new
            {
                ms = 1,
                second = 1000,
                minute = 60000,
                hour = 3.6e+6,
                day = 8.64e+7,
                week = 6.048e+8,
                month = 2.628e+9,
                year = 3.156e+10,
                decade = 3.154e+11
            },

            now = new NativeFunction((options) =>
            {
                return new NumberValue(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            }, new Options("now")),
        }));
    }
}
