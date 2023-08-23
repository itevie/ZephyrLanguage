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
    internal partial class Statements
    {
        public static RuntimeValue EvaluateWhileStatement(WhileStatement statement, Environment environment)
        {
            RuntimeValue test = Interpreter.Evaluate(statement.Test, environment);

            int currentIterations = 0;

            while (Helpers.EvaluateTruhyValueHelper(test) == true)
            {
                currentIterations++;
                
                // Check if reached max
                if (currentIterations >= Program.Options.MaxLoopIterations && Program.Options.NoIterationLimit == false)
                {
                    throw new RuntimeException(new()
                    {
                        Location = statement.Location,
                        Error = $"Loop reached max iteration limit (change with --max-iterations flag)"
                    });
                }

                Interpreter.Evaluate(statement.Body, environment);
                test = Interpreter.Evaluate(statement.Test, environment);
            }

            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
