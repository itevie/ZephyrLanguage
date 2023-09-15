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
        public static RuntimeValue EvaluateBreakStatement(BreakStatement statement, Environment env)
        {
            // Check if can break here
            try
            {
                env.LookupVariable("~break");
            } catch
            {
                throw new RuntimeException(new()
                {
                    Error = "Cannot break here",
                    Location = statement.Location
                });
            }

            env.AssignVariable("~break", Values.Helpers.Helpers.CreateBoolean(true), true);

            return new NullValue()
            {
                WasBroken = true,
                Value = null,
            };
        }
    }
}
