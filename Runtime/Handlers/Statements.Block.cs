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
        public static RuntimeValue EvaluateBlockStatement(BlockStatement block, Environment environment)
        {
            Environment env = new Environment(environment);
            RuntimeValue lastValue = new Values.NullValue();

            foreach (Expression expression in block.Body)
            {
                lastValue = Interpreter.Evaluate(expression, env);
            }

            return lastValue;
        }
    }
}
