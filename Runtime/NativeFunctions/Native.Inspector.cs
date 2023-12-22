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
        /// <summary>
        /// This module is for inspecting runtime values or other such things
        /// </summary>
        public static Package InspectorPackage = new Package("Inspect", new Values.ObjectValue(new
        {
            getLocation = new NativeFunction(options =>
            {
                RuntimeValue value = options.Arguments[0];

                return new ObjectValue(new
                {
                    charStart = value.Location.TokenStart,
                    charEnd = value.Location.TokenEnd,
                    line = value.Location.Line,
                });
            }, new Options("getLocation", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Any), "val")
            })),

            getSource = new NativeFunction(options =>
            {
                RuntimeValue value = options.Arguments[0];

                return value.Location.Source != null && Lexer.Lexer.AllSources.ContainsKey(value.Location.Source)
                    ? new StringValue(Lexer.Lexer.AllSources[value.Location.Source])
                    : new NullValue();
            }, new Options("getSource", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Any), "val")
            })),

            visualizeType = new NativeFunction(options =>
            {
                RuntimeValue value = options.Arguments[0];

                return new StringValue(Values.Helpers.VisualiseType(value.Type));
            }, new Options("visualizeType", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Any), "val")
            })),
        }));
    }
}
