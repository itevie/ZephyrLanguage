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
        public static RuntimeValue EvaluateAwaitExpression(AwaitExpression expression, Environment environment)
        {
            // Collet right
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

            // Await it
            if (right.Type.TypeName == Values.ValueType.Future)
            {
                return ((FutureValue)right).Task.GetAwaiter().GetResult();
            }

            throw new RuntimeException($"Cannot await a {Values.Helpers.VisualiseType(right.Type)}", expression.Right.Location);
        }
    }
}
