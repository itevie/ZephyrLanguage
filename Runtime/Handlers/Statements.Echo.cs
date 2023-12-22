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
        public static RuntimeValue EvaluateEchoStatement(EchoStatement statement, Environment environment)
        {
            Console.WriteLine(Interpreter.Evaluate(statement.Value, environment).Visualise());
            return new NullValue();
        }
    }
}
