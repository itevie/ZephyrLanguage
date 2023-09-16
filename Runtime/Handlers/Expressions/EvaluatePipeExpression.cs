using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Parser.AST.Expressions;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluatePipeExpression(PipeExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);

            // Create args
            List<RuntimeValue> args = new()
            {
                left
            };

            // Get function
            RuntimeValue function; 
            
            if (expression.Right.Kind == Kind.CallExpression)
            {
                function = Interpreter.Evaluate(((CallExpression)expression.Right).Caller, environment);

                foreach (Expression expr in ((CallExpression)expression.Right).Arguments)
                {
                    args.Add(Interpreter.Evaluate(expr, environment));
                }
            } else
            {
                function = Interpreter.Evaluate(expression.Right, environment);
            }

            // Evaluate function
            RuntimeValue givenValue;

            if (function.Type == Values.ValueType.Function)
            {
                givenValue = Helpers.EvaluateFunctionHelper((FunctionValue)function, args, environment, expression);
            } else if (function.Type == Values.ValueType.NativeFunction)
            {
                givenValue = Helpers.EvaluateFunctionHelper((NativeFunction)function, args, environment, expression);
            } else
            {
                throw new RuntimeException(new()
                {
                    Error = $"Expected function",
                    Location = expression.Right.Location
                });
            }

            return givenValue;
        }
    }
}
