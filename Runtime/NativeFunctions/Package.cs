using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal class Package
    {
        public string Name;
        public ObjectValue Object;

        public Package(string name, ObjectValue @object)
        {
            Name = name;
            Object = @object;
        }
    }
}
