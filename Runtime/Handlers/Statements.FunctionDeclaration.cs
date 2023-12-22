using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateFunctionDeclaration(FunctionDeclaration statement, Environment environment)
        {
            Environment functionEnv = new Environment(environment);

            FunctionValue function = new FunctionValue(functionEnv, statement.Body, new VariableType(statement.ReturnType, environment, statement.ReturnType.Location), statement.Location)
            {
                Name = statement.Name.Symbol,
                Parameters = statement.Parameters
            };

            return new VariableReference(
                environment.DeclareVariable(statement.Name.Symbol, function, new VariableSettings(function.Type, statement.Location), statement.Location)
            );
        }
    }
}
