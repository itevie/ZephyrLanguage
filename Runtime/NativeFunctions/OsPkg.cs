using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Packages
    {
        public static NonDefaultPackage OsPkg = new("System", new
        {
            tempDirectory = Path.GetTempPath(),
            OSName = RuntimeInformation.OSDescription,
            getOSName = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                string type = "";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    type = "windows";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    type = "linux";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    type = "osx";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                    type = "freebsd";
                else type = "unknown";

                return Helpers.CreateString(type);
            }, options: new()
            {
                Name = "getOSName",
            }),
        }, () =>
        {
            // Check permissions
            if (Program.Options.CanAccessSystemDetails == false)
            {
                return $"This package cannot be used as \"CanAccessSystemDetails\" is denied.\nIf you'd like to use it, run this program with --system-details=true";
            }
            return null;
        });
    }
}
