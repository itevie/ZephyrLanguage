using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static Values.RuntimeValue EvaluateFunctionDeclaration(Parser.AST.FunctionDeclaration declaration, Environment environment)
        {
            List<string> parameters = new();

            foreach (Expression parameter in declaration.Parameters)
            {
                // Check if parameter is the function nane
                if (((Identifier)parameter).Symbol == ((Identifier?)declaration?.Name)?.Symbol)
                {
                    throw new RuntimeException_new()
                    {
                        Location = Helpers.GetLocation(parameter.Location, declaration.Location),
                        Error = $"Cannot define a parameter with the same name as the declared function"
                    };
                }

                parameters.Add(((Identifier)parameter).Symbol);
            }

            Values.FunctionValue func = new(environment)
            {
                Name = declaration.Name != null ? ((Identifier)declaration.Name).Symbol : $"~Anonymous::{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{Guid.NewGuid()}",
                Parameters = parameters,
                DeclarationEnvironment = environment,
                Body = declaration.Body,
            };

            func.Location = declaration.Location;

            return environment.DeclareVariable(func.Name, func, new VariableSettings(), declaration);
        }
    }
}
