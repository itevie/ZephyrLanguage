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
        public static RuntimeValue EvaluateTryStatement(TryStatement statement, Environment environment)
        {
            try
            {
                Interpreter.Evaluate(statement.Body, new Environment(environment));
            } catch (RuntimeException e)
            {
                if (e.GetType() == typeof(ContinueStatement) || e.GetType() == typeof(ReturnStatement) || e.GetType() == typeof(BreakStatement))
                {
                    throw;
                }

                // Check if there is a catch
                if (statement.Catch != null)
                {
                    // Create env
                    Environment env = new Environment(environment);

                    // Check if declares error
                    if (statement.CatchIdentifier != null)
                    {
                        env.DeclareVariable(statement.CatchIdentifier.Symbol, new ObjectValue(new
                        {
                            message = e.Message,
                            location = new
                            {
                                charStart = e.Location.TokenStart,
                                starEnd = e.Location.TokenEnd,
                                line = e.Location.Line,
                            },
                            asString = e.Visualise(),
                        }), new VariableSettings(new VariableType(Values.ValueType.Object), statement.CatchIdentifier.Location), statement.CatchIdentifier.Location);
                    }

                    Interpreter.Evaluate(statement.Catch, env);
                }
            }

            return new NullValue();
        }
    }
}
