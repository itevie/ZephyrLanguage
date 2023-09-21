using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST.Statements;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateSwitchStatement(SwitchStatement statement, Environment env)
        {
            // Compute the test
            RuntimeValue test = Interpreter.Evaluate(statement.Test, env);

            // Switch environment
            Environment switchEnv = new Environment(env);

            bool passThroughed = false;

            // Loop through the tests
            foreach (Parser.AST.Expressions.SwitchCase switchCase in statement.Cases)
            {
                // Compute the test
                RuntimeValue switchTest = switchCase.IsDefault || passThroughed ? test : Interpreter.Evaluate(switchCase.Test, switchEnv);
                passThroughed = false;

                // returns true if it was passthrough'd
                bool executeCase()
                {
                    // Set ~passthrough
                    switchEnv.DeclareVariable("~passthrough", Values.Helpers.Helpers.CreateBoolean(false), VariableSettingsPresets.Secured);
                    EvaluateBlockStatement((Parser.AST.BlockStatement)switchCase.Success, switchEnv);
                    return ((Values.BooleanValue)switchEnv.LookupVariable("~passthrough")).Value;
                }

                bool execute = false;

                // Check the test - base test just equals
                if (Helpers.EvaluateSimpleComparisonHelper(test, switchTest, Lexer.Syntax.Operators.ComparisonOperators["Equals"].Symbol))
                {
                    execute = true;
                }

                if (execute)
                {
                    bool wasPassed = executeCase();
                    if (wasPassed)
                    {
                        passThroughed = true;
                        continue;
                    } else
                    {
                        break;
                    }
                }
            }

            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
