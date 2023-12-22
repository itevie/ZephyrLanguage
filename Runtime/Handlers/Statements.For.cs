using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Exceptions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateForStatement(ForStatement statement, Environment environment)
        {
            Environment loopEnv = new Environment(environment);
            Interpreter.Evaluate(statement.Declaration, loopEnv);

            while (Values.Helpers.IsValueTruthy(Interpreter.Evaluate(statement.Test, loopEnv)))
            {
                try
                {
                    Interpreter.Evaluate(statement.Body, loopEnv);
                }
                catch (BreakException _)
                {
                    break;
                }
                catch (ContinueException _)
                {
                    continue;
                }
                Interpreter.Evaluate(statement.Increment, loopEnv);
            }

            return new NullValue();
        }
    }
}
