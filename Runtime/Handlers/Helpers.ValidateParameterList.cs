using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.NativeFunctions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static bool ValidateParameterList(List<RuntimeValue> parameters, List<FunctionParameter> compareWith, Environment environment, Location from, Environment? funcEnv = null)
        {
            // Check if params
            if (compareWith.Count != 0 && compareWith[0].IsParams)
            {
                // Check all
                foreach (RuntimeValue val in parameters)
                {
                    Values.Helpers.TypeMatches(Values.Helpers.DecreaseArrayDepth(new VariableType(compareWith[0].Type, funcEnv ?? environment, from)), val);
                }

                return true;
            }

            // Check lengths
            if (parameters.Count != compareWith.Count)
                throw new RuntimeException($"Parameter miscount, expected {compareWith.Count} but got {parameters.Count}", from);

            // Loop through them
            for (int i = 0; i != parameters.Count; i++)
            {
                // Check type
                string? valid = Values.Helpers.TypeMatches(new VariableType(compareWith[i].Type, funcEnv ?? environment, from), parameters[i]);

                if (valid != null)
                    throw new RuntimeException($"Error with parameter {i + 1}: {valid}", from);
            }

            return true;
        }

        public static bool ValidateParameterList(List<RuntimeValue> parameters, List<Parameter> compareWith, Location from)
        {
            // Check lengths
            if (parameters.Count != compareWith.Count)
                throw new RuntimeException($"Parameter miscount, expected {compareWith.Count} but got {parameters.Count}", from);

            // Loop through them
            for (int i = 0; i != parameters.Count; i++)
            {
                // Check type
                string? valid = Values.Helpers.TypeMatches(compareWith[i].Type, parameters[i]);

                if (valid != null)
                    throw new RuntimeException($"Error with parameter {i + 1}: {valid}", from);
            }

            return true;
        }
    }
}
