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
        public static RuntimeValue EvaluateRangeExpression(RangeExpression expression, Environment environment)
        {
            // Collect values
            RuntimeValue startValue = Interpreter.Evaluate(expression.Left, environment);
            RuntimeValue endValue = Interpreter.Evaluate(expression.Right, environment);
            RuntimeValue? stepValue = expression.Step == null ? null : Interpreter.Evaluate(expression.Step, environment);

            // Check types
            if (startValue.Type.TypeName != Values.ValueType.Number)
                throw new RuntimeException($"Range start must be a number", expression.Left.Location);
            if (endValue.Type.TypeName != Values.ValueType.Number)
                throw new RuntimeException($"Range end must be a number", expression.Left.Location);
            if (stepValue != null && stepValue.Type.TypeName != Values.ValueType.Number)
                throw new RuntimeException($"Range step must be a number", expression.Left.Location);

            // Collect proper types
            double start = ((NumberValue)startValue).Value;
            double end = ((NumberValue)endValue).Value;
            double step = stepValue == null ? 1 : ((NumberValue)stepValue).Value;

            // Compute how big it will be
            double steps = Math.Abs(start - end) / step;
            double singleStep = start + step;

            // Check infinite loop
            if ((start > singleStep) && (start < end))
                throw new RuntimeException($"This range appears to have no end", expression.Location);

            // Start to compute the range
            List<RuntimeValue> items = new List<RuntimeValue>();

            for (double i = start; i < end + (expression.Uninclusive ? 0 : 1); i += step)
            {
                items.Add(new NumberValue(i));
            }

            // Done
            return new ArrayValue(items, new VariableType(Values.ValueType.Array)
            {
                IsArray = true,
                ArrayDepth = 1,
                ArrayType = Values.ValueType.Number
            });
        }
    }
}
