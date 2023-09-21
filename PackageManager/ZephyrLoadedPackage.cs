using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.PackageManager
{
    /// <summary>
    /// This simply contains a reference to where a package is located within zephyr_packages
    /// </summary>
    internal class ZephyrLoadedPackage
    {
        public string EntryPoint;

        public ZephyrLoadedPackage(string path)
        {
            EntryPoint = path;
        }
    }
}
