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
        public static RuntimeValue EvaluateIfStatement(IfStatement ifStatement, Environment environment)
        {
            // Check test
            RuntimeValue value = Interpreter.Evaluate(ifStatement.Test, environment);
            bool isSuccess = Values.Helpers.IsValueTruthy(value);

            if (isSuccess)
            {
                // Run the body
                return Interpreter.Evaluate(ifStatement.Success, new Environment(environment));
            } else if (ifStatement.Alternate != null)
            {
                // Run the alternate
                return Interpreter.Evaluate(ifStatement.Alternate, new Environment(environment));
            }

            return new Values.NullValue();
        }
    }
}
