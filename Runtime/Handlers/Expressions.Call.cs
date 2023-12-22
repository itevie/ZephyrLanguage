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
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateCallExpression(CallExpression expression, Environment environment)
        {
            // Find function
            RuntimeValue callee = Interpreter.Evaluate(expression.Callee, environment);

            // Check type
            if (callee.Type.TypeName != Values.ValueType.Function && 
                callee.Type.TypeName != Values.ValueType.NativeFunction &&
                callee.Type.TypeName != Values.ValueType.AsyncNativeFunction)
                throw new RuntimeException($"Cannot call a {callee.Type.TypeName}", expression.Callee.Location);

            // Collect values
            List<RuntimeValue> values = new List<RuntimeValue>();

            foreach (Expression expr in expression.Parameters)
            {
                if (expr.Kind == Kind.SpreadExpression)
                {
                    values.AddRange(Helpers.GetSpreadValues((SpreadExpression)expr, environment));
                }
                else values.Add(Interpreter.Evaluate(expr, environment));
            }

            // Check native or not
            if (callee.Type.TypeName == Values.ValueType.NativeFunction)
            {
                NativeFunction nativeFn = (NativeFunction)callee;
                return Helpers.ExecuteNativeFunction(nativeFn, values, environment, expression.Location);
            } else if (callee.Type.TypeName == Values.ValueType.AsyncNativeFunction)
            {
                AsyncNativeFunction nativeFn = (AsyncNativeFunction)callee;
                return Helpers.ExecuteNativeFunction(nativeFn, values, environment, expression.Location);
            }

            // It is not native
            FunctionValue calleeFn = (FunctionValue)callee;
            return Helpers.ExecuteZephyrFunction(calleeFn, values, environment, expression);
        }
    }
}
