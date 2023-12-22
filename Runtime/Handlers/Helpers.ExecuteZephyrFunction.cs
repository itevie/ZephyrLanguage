using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Exceptions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue ExecuteZephyrFunction(FunctionValue function, List<RuntimeValue> arguments, Environment environment, CallExpression? expression = null)
        {
            // Apply decorators
            List<FunctionValue> decorators = new List<FunctionValue>(function.Decorators);
            foreach (FunctionValue func in decorators)
            {
                FunctionValue newFunc = new FunctionValue(function.Environment, function.Body, function.ReturnType, function.Location)
                {
                    Parameters = function.Parameters,
                };
                function = (FunctionValue)ExecuteZephyrFunction(func, new List<RuntimeValue> { newFunc }, environment, expression);
            }

            // Check types
            Helpers.ValidateParameterList(arguments, function.Parameters, environment, expression?.Location ?? Location.UnknownLocation, function.Environment);

            // Create new env
            Environment functionEnv = new Environment(function.Environment);

            functionEnv.DeclareVariable("this", function, new VariableSettings(function.Type, function.Location), function.Location);

            // Check if is params
            if (function.Parameters.Count != 0 && function.Parameters[0].IsParams)
            {
                VariableType type = new VariableType(function.Parameters[0].Type, environment, function.Parameters[0].Location);
                functionEnv.DeclareVariable(
                    function.Parameters[0].Name.Symbol,
                    new ArrayValue(arguments, type),
                    new VariableSettings(type, function.Parameters[0].Location),
                    function.Parameters[0].Location
                );
            } else
            {
                // Add parameters
                for (int i = 0; i != function.Parameters.Count; i++)
                {
                    functionEnv.DeclareVariable(
                        function.Parameters[i].Name.Symbol,
                        arguments[i],
                        new VariableSettings(
                            arguments[i].Type,
                            arguments[i].Location ?? Location.UnknownLocation),
                        arguments[i].Location ?? Location.UnknownLocation
                    );
                }
            }

            RuntimeValue value = new VoidValue();

            try
            {
                StackContainer.PushToStack(new StackItem(function, expression?.Location ?? Location.UnknownLocation));
                BlockStatement block = (BlockStatement)function.Body;
                Statements.EvaluateBlockStatement(block, functionEnv);
            }
            catch (ReturnException exception)
            {
                // Check value
                string? valid = Values.Helpers.TypeMatches(function.ReturnType, exception.Value);

                if (valid != null)
                    throw new RuntimeException($"Expected return type of {Values.Helpers.VisualiseType(function.ReturnType)}, but got {Values.Helpers.VisualiseType(exception.Value.Type)}", exception.Location);

                value = exception.Value;
            }


            try
            {
                Values.Helpers.TypeMatches(function.ReturnType, value);
            } catch (RuntimeException e)
            {
                throw new RuntimeException($"Invalid return type: {e.Message}", e.Location);
            }

            value.Location = expression?.Callee.Location ?? Location.UnknownLocation;

            StackContainer.PopFromStack();

            return value;
        }
    }
}
