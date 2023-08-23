using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class ZephyrPackage
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("version")]
        public string Version { get; set; } = "1.0.0";

        [JsonProperty("entryPoint")]
        public string EntryPoint { get; set; } = "index.zr";

        [JsonProperty("dependencies")]
        public Dictionary<string, ZephyrPackageDependency> Dependencies { get; set; } = new();
    }
}
