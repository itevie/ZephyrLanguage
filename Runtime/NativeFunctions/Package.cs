using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.NativeFunctions
{
    /// <summary>
    /// A "Package" is a in-built Zephyr package. It is automatically added to global environment on start-up
    /// They are added as an object into the environment.
    /// </summary>
    internal class Package
    {
        /// <summary>
        /// The name of the package, this is the variable created in the environment
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The object containing all of the package's properties
        /// </summary>
        public object Object { get; set; }

        public Package(string name, object @object)
        {
            Name = name;
            Object = @object;
        }
    }

    /// <summary>
    /// This is similar to a Package, but it is not imported by default, this is because they have validation on whether or not they can be imported
    /// For example, the Windows package can only be imported on the Windows OS.
    /// </summary>
    internal class NonDefaultPackage
    {
        /// <summary>
        /// The name of the package, this is the variable created in the environment
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The object containing all of the package's properties
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// The validation function, the string? is the returned error. If it is null - no error happened
        /// If a string is returned - an error happened
        /// </summary>
        public Func<string?> Validate { get; set; }

        public NonDefaultPackage(string name, object @object, Func<string?> validate)
        {
            Name = name;
            Object = @object;
            Validate = validate;
        }   
    }
}
