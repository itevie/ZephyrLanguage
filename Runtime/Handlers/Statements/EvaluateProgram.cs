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
        public static RuntimeValue EvaluateProgram(Parser.AST.Program program, Environment environment)
        {
            RuntimeValue lastEvaulated = Values.Helpers.Helpers.CreateNull();

            foreach (Parser.AST.Expression statement in program.Body)
            {
                lastEvaulated = Interpreter.Evaluate(statement, environment);
            }

            return lastEvaulated;
        }
    }
}
