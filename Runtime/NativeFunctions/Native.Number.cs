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
        public static Package NumberPackage = new Package("Number", new Values.ObjectValue(new
        {
            parse = new NativeFunction((options) =>
            {
                string toParse = ((StringValue)options.Arguments[0]).Value;
                double output;
                bool success = Double.TryParse(toParse, out output);

                // Check success
                if (!success)
                    throw new RuntimeException($"Failed to parse string as number: {toParse}", options.Location);

                // Success
                return new NumberValue(output);
            }, new Options("parse", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "str")
            })),
        }));
    }
}