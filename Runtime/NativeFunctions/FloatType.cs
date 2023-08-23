using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package FloatTypePkg = new Package("Float", new
        {
            parse = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string text = ((StringValue)args[0]).Value;
                float val = 0;

                if (float.TryParse(text, out val) == false)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(args[0].Location, expr.Location),
                        Error = $"Invalid float"
                    });
                }

                return Helpers.CreateFloat(val);
            }, options: new()
            {
                Name = "parse",
                Parameters =
                {
                    new()
                    {
                        Name = "stringFloat",
                        Type = Values.ValueType.String
                    }
                }
            }),
        });
    }
}
