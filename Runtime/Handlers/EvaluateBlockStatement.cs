using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateBlockStatement(BlockStatement block, Environment environment)
        {
            RuntimeValue result = Values.Helpers.Helpers.CreateNull();

            // Create scope
            Environment scope = new Environment(environment);
            
            foreach (Expression statement in block.Body)
            {
                result = Interpreter.Evaluate(statement, scope);

                // Check if return
                if (result.IsReturn)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
