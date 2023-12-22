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
        public static RuntimeValue EvaluateInExpression(InExpression expression, Environment environment)
        {
            // Collect values
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

            if (right.Type.TypeName == Values.ValueType.Object)
            {
                // Expect string
                if (left.Type.TypeName != Values.ValueType.String)
                    throw new RuntimeException($"Expected string on left side", expression.Left.Location);
                return new BooleanValue(right.Properties.ContainsKey(((StringValue)left).Value));
            }

            // Enummerate
            ArrayValue array = right.Enummerate(expression.Right.Location);

            // Loop through them
            foreach (dynamic value in array.Items)
            {
                try
                {
                    if (value.Value == ((dynamic)left).Value)
                        return new BooleanValue(true);
                } catch { }
            }

            return new BooleanValue(false);
        }
    }
}
