using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Exceptions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateLoopStatement(LoopStatement statement, Environment environment)
        {
            RuntimeValue lastValue = new Values.NullValue();
            
            while (true)
            {
                try
                {
                    lastValue = Interpreter.Evaluate(statement.Body, new Environment(environment));
                } catch (BreakException _)
                {
                    break;
                } catch (ContinueException _)
                {
                    continue;
                }
            }

            return lastValue;
        }
    }
}
