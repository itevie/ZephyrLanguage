using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateRuntimeStatment(ReturnStatement statement, Environment environment)
        {
            RuntimeValue value = statement.Value != null ? Interpreter.Evaluate(statement.Value, environment) : Values.Helpers.Helpers.CreateNull();
            value.IsReturn = true;

            try
            {
                environment.AssignVariable("~return", value, true, statement);
            }
            catch
            {
                throw new RuntimeException(new()
                {
                    Location = statement.Location,
                    Error = "Cannot return here"
                });
            }

            return value;
        }
    }
}
