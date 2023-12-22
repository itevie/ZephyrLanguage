using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateProgram(Parser.AST.Program program, Environment environment)
        {
            RuntimeValue lastEvaluated = new NullValue();

            foreach (Expression expression in program.Body)
            {
                lastEvaluated = Interpreter.Evaluate(expression, environment);
            }

            return lastEvaluated;
        }
    }
}
