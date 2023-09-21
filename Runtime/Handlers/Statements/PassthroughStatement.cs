using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Parser.AST.Statements;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluatePassthroughStatement(PassthroughStatement statement, Environment env)
        {
            // Check if can break here
            try
            {
                env.LookupVariable("~passthrough");
            }
            catch
            {
                throw new RuntimeException(new()
                {
                    Error = "Cannot passthrough here",
                    Location = statement.Location
                });
            }

            env.AssignVariable("~passthrough", Values.Helpers.Helpers.CreateBoolean(true), true);

            return new NullValue()
            {
                WasBroken = true,
                Value = null,
            };
        }
    }
}
