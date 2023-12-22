using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.NativeFunctions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static bool ValidateNativeOptions(List<RuntimeValue> args, Options options, Location location)
        {
            if (options.IsManaged) return true;
            if (options.AllOfType != null)
            {
                for (int i = 0; i != args.Count; i++)
                {
                    if (!Values.Helpers.CompareTypes(args[i].Type, options.AllOfType))
                        throw new RuntimeException($"Error with parameter {i + 1}: All parameters must be of type {Values.Helpers.VisualiseType(options.AllOfType)}", location);
                }

                return true;
            }

            return Helpers.ValidateParameterList(args, options.Parameters, location);
        }
    }
}
