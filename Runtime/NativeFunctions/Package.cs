using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.NativeFunctions
{
    internal class Package
    {
        public string Name { get; set; }
        public object Object { get; set; }

        public Package(string name, object @object)
        {
            Name = name;
            Object = @object;
        }
    }

    internal class NonDefaultPackage
    {
        public string Name { get; set; }
        public object Object { get; set; }

        public Func<string?> Validate { get; set; }

        public NonDefaultPackage(string name, object @object, Func<string?> validate)
        {
            Name = name;
            Object = @object;
            Validate = validate;
        }   
    }
}
