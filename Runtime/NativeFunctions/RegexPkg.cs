using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package RegexPkg = new("Regex", new
        {
            isMatch = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string toMatch = ((StringValue)args[0]).Value;
                string regex = ((StringValue)args[1]).Value;
                //string flags = ((StringValue)args[2]).Value;

                Regex reg = new(regex);

                return Helpers.CreateBoolean(Regex.IsMatch(toMatch, regex));
            }, options: new()
            {
                Name = "match",
                Parameters =
                {
                    new()
                    {
                        Name = "toMatch",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "regex",
                        Type = Values.ValueType.String
                    },
                    /*new()
                    {
                        Name = "regexFlags",
                        Type = Values.ValueType.String
                    }*/
                }
            })
        });
    }
}
