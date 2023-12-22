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
        public static RuntimeValue EvaluatePipeExpression(PipeExpression expression, Environment environment)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);

            // Create args
            List<RuntimeValue> args = new List<RuntimeValue>()
            {
                left,
            };

            // Gather function
            RuntimeValue function;

            if (expression.Right.Kind == Kind.CallExpression)
            {
                function = Interpreter.Evaluate(((CallExpression)expression.Right).Callee, environment);

                foreach (Expression expr in ((CallExpression)expression.Right).Parameters)
                {
                    args.Add(Interpreter.Evaluate(expr, environment));
                }
            } else
            {
                function = Interpreter.Evaluate(expression.Right, environment);
            }

            // Check type
            if (function.Type.TypeName != Values.ValueType.Function && function.Type.TypeName != Values.ValueType.NativeFunction)
                throw new RuntimeException($"Cannot call a {function.Type.TypeName}", expression.Right.Location);

            // Check if native
            if (function.Type.TypeName == Values.ValueType.NativeFunction)
            {
                return Helpers.ExecuteNativeFunction((NativeFunction)function, args, environment, expression.Right.Location);
            } else
            {
                return Helpers.ExecuteZephyrFunction((FunctionValue)function, args, environment);
            }
        }
    }
}
