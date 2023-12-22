using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime
{
    internal static class ImportedModules
    {
        public static Dictionary<string, Environment> Modules = new Dictionary<string, Environment>();

        public static void AddModule(string fileName, Environment environment)
        {
            Modules.Add(fileName, environment);
        }

        public static bool HasModule(string fileName)
        {
            return Modules.ContainsKey(fileName);
        }

        public static Environment GetModule(string fileName)
        {
            return Modules[fileName];
        }
    }
}
