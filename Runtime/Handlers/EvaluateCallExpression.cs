using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateCallExpression(Parser.AST.CallExpression expression, Environment environment)
        {
            // Get the values
            List<RuntimeValue> arguments = new List<RuntimeValue>();
            RuntimeValue function = Interpreter.Evaluate(expression.Caller, environment);

            // Load arguments
            foreach (Expression argument in expression.Arguments)
            {
                arguments.Add(Interpreter.Evaluate(argument, environment));
            }

            // Check if it is a native function
            if (function.Type == Values.ValueType.NativeFunction)
            {
                if (((NativeFunction)function).IsTypeCall)
                {
                    arguments.Insert(0, ((NativeFunction)function).TypeCallValue);
                    return Helpers.EvaluateFunctionHelper(((NativeFunction)function), arguments, environment, expression.Caller);
                }
                // Execute native function
                return Helpers.EvaluateFunctionHelper(((NativeFunction)function), arguments, environment, expression.Caller);
            } else if (function.Type == Values.ValueType.Function)
            {
                RuntimeValue val = Helpers.EvaluateFunctionHelper(((FunctionValue)function), arguments, environment, expression);
                return val;
            }
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Caller.Location,
                    Error = $"Cannot call a {function.Type}"
                });
            }
        }
    }
}
