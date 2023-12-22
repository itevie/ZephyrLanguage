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
        public static RuntimeValue EvaluateReturnStatement(ReturnStatement statement, Environment environment)
        {
            throw new Exceptions.ReturnException(statement.Location, Interpreter.Evaluate(statement.Value, environment));
        }
    }
}
