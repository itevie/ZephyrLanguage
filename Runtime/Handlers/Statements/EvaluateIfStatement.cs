using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateIfStatement(Parser.AST.IfStatement statement, Environment environment)
        {
            RuntimeValue value = Interpreter.Evaluate(statement.Test, environment);

            // Check if truthy
            bool isTruthy = Helpers.EvaluateTruhyValueHelper(value);

            RuntimeValue result = Values.Helpers.Helpers.CreateNull();

            // Check
            if (isTruthy)
            {
                result = Interpreter.Evaluate(statement.Success, environment);
            } else
            {
                // Check for alternative
                if (statement.Alternate != null)
                {
                    result = Interpreter.Evaluate(statement.Alternate, environment);
                }
            }

            return result;
        }
    }
}
