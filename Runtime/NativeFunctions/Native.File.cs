using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package FilePackage = new Package("File", new Values.ObjectValue(new
        {
            readAllText = new NativeFunction((options) =>
            {
                string origName = ((StringValue)options.Arguments[0]).Value;
                string fileName = ZephyrPath.Resolve(options.Environment.Directory, origName);

                // Check if it exists
                if (File.Exists(fileName) == false)
                    throw new RuntimeException($"The file {origName} does not exist", options.Location);

                return new StringValue(File.ReadAllText(fileName));
            }, new Options("readAllText", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "fileName"),
            })),


            exists = new NativeFunction((options) =>
            {
                string origName = ((StringValue)options.Arguments[0]).Value;
                string fileName = ZephyrPath.Resolve(options.Environment.Directory, origName);

                return new BooleanValue(File.Exists(fileName));
            }, new Options("exists", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "fileName"),
            })),
        }));
    }
}
