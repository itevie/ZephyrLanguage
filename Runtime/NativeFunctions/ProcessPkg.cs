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
        public static Package ProcessPkg = new Package("Process", new
        {
            /* FUNCTIONS */
            enableDebug = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Program.Options.Debug = true;
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "enableDebug",
            }),
            enableVerbose = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Program.Options.Verbose = true;
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "enableVerbose",
            }),

            exit = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Program.Finish();
                return Helpers.CreateNull();
            }, options: new() { Name = "exit" }),

            currentDirectory = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(env.Directory);
            }),

            /* VALUES */
            iterationLimit = Helpers.CreateInteger(Program.Options.MaxLoopIterations),
            noIterationLimit = Helpers.CreateBoolean(Program.Options.NoIterationLimit),
            permissions = Helpers.CreateObject(new
            {
                canSpawnProcesses = Helpers.CreateBoolean(Program.Options.CanSpawnProcesses),
                canAccessSystemDetails = Helpers.CreateBoolean(Program.Options.CanAccessSystemDetails),
                files = Helpers.CreateObject(new
                {
                    canRead = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('r')),
                    canWrite = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('w')),
                    canDeleteFiles = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('d')),
                    canDeleteDirectories = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('x')),
                    canCreate = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('c')),
                    none = Helpers.CreateBoolean(Program.Options.FileAccessFlags.Contains('n')),
                })
            })
        });
    }
}
