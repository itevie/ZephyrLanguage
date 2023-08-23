using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class ZephyrPackageDependency
    {

        [JsonProperty("version")]
        public string Version { get; set; } = "";
    }
}
