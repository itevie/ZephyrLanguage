using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Zephyr.Runtime.Values.Helpers;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Packages
    {
        public static NonDefaultPackage ChildProcessPkg = new NonDefaultPackage("ChildProcess", new
        {
            executeFile = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Process.Start(((StringValue)args[0]).Value);
                return Helpers.CreateNull();
            }, options: new Values.NativeFunctionOptions()
            {
                Parameters =
                {
                    new()
                    {
                        Name = "filePath",
                        Type = Values.ValueType.String
                    }
                }
            })
        }, () =>
        {
            if (Program.Options.CanSpawnProcesses == false)
            {
                return "Cannot import this module as --can-spawn-processes is denied";
            }

            return null;
        });
    }
}
