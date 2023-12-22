using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Literals
    {
        public static RuntimeValue EvaluateArrayLiteral(ArrayLiteral literal, Environment environment)
        {
            List<RuntimeValue> values = new List<RuntimeValue>();

            foreach (Expression expression in literal.Items)
            {
                values.Add(Interpreter.Evaluate(expression, environment));
            }

            return new ArrayValue(values, VariableType.AnyArray);
        }
    }
}
