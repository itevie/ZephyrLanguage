using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class ZephyrLoadedPackage
    {
        public string EntryPoint;

        public ZephyrLoadedPackage(string path)
        {
            EntryPoint = path;
        }
    }
}
