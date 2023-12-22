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
        public static RuntimeValue EvaluateFunctionExpression(FunctionExpression statement, Environment environment)
        {
            Environment functionEnv = new Environment(environment);

            FunctionValue function = new FunctionValue(functionEnv, statement.Body, new VariableType(statement.ReturnType, environment, statement.ReturnType.Location), statement.Location)
            {
                Name = statement.Name.Symbol,
                Parameters = statement.Parameters
            };

            return function;
        }
    }
}
