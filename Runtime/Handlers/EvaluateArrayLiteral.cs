using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateArrayLiteral(ArrayLiteral expression, Environment environment)
        {
            List<RuntimeValue> values = new();

            foreach (Expression item in expression.Items)
            {
                RuntimeValue val = Interpreter.Evaluate(item, environment);

                // Check if enumerable
                if (val.Type == Values.ValueType.Enumerable)
                {
                    foreach (RuntimeValue enumerableValue in ((EnumerableValue)val).Values)
                    {
                        values.Add(enumerableValue);
                    }
                } else
                {
                    values.Add(val);
                }
            }

            ArrayValue value = Values.Helpers.Helpers.CreateArray(values, expression.Type);
            value.Location = Helpers.GetLocation(expression.FullExpressionLocation, expression.Location);
            return value;
        }
    }
}
