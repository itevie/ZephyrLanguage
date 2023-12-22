using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateLambdaExpression(LambdaExpression statement, Environment environment)
        {
            Environment functionEnv = new Environment(environment);

            List<FunctionParameter> parameters = new List<FunctionParameter>();
            List<VariableType> generics = new List<VariableType>();

            foreach (Identifier identifier in statement.Arguments)
            {
                parameters.Add(new FunctionParameter(identifier.Location, identifier, new Parser.AST.Type(Values.ValueType.Any, true, identifier.Location)));
                generics.Add(new VariableType(Values.ValueType.Any));
            }

            generics.Add(new VariableType(Values.ValueType.Any));

            FunctionValue function = new FunctionValue(functionEnv, statement.Body, new VariableType(Values.ValueType.Any), statement.Location)
            {
                Name = $"~lambda",
                Parameters = parameters,
                Type = new VariableType(Values.ValueType.Function)
                {
                    GenericsList = generics,
                }
            };

            return function;
        }
    }
}
