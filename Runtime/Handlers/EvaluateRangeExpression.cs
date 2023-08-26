using Newtonsoft.Json.Linq;
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
        public static RuntimeValue EvaluateRangeExpression(RangeExpression expression, Environment environment)
        {
            RuntimeValue startVal = Interpreter.Evaluate(expression.Start, environment);
            RuntimeValue endVal = Interpreter.Evaluate(expression.End, environment);
            RuntimeValue? stepVal = expression.Step != null ? Interpreter.Evaluate(expression.Step, environment) : null;

            // Check types
            if (startVal.Type != Values.ValueType.Int)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Start.Location,
                    Error = "Range start must be of type integer"
                });
            } else if (endVal.Type != Values.ValueType.Int)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.End.Location,
                    Error = "Range end must be of type integer"
                });
            } else if (stepVal != null && stepVal.Type != Values.ValueType.Int)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Step.Location,
                    Error = "Range step must be of type integer"
                });
            }

            int start = (int)((IntegerValue)startVal).Value;
            int end = (int)((IntegerValue)endVal).Value;
            int step = (int?)((IntegerValue?)stepVal)?.Value ?? 1;

            // Execute how big it will be 
            int amount = Math.Abs(start - end) / step;

            if (amount > Program.Options.MaxLoopIterations && Program.Options.NoIterationLimit == false)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Location,
                    Error = $"This range expression exceeds the maximum loop iterations ({amount} > {Program.Options.MaxLoopIterations})"
                });
            }

            int oneStep = start + step;

            // Check inf loop
            if ((start > oneStep) && (start < end))
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Location,
                    Error = $"This range expression appears to have no end"
                });
            }

            List<int> range = new();

            for (int i = start; i < end + (expression.Uninclusive ? 0 : 1); i += step)
            {
                range.Add(i);
            }

            // Turn into enumerable
            List<RuntimeValue> values = new(range.Select(x => Values.Helpers.Helpers.CreateInteger(x)));

            return new EnumerableValue()
            {
                Values = values
            };
        }
    }
}
