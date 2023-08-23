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
        public static RuntimeValue EvaluateTryStatement(TryStatement statement, Environment environment)
        {
            try
            {
                Interpreter.Evaluate(statement.Body, environment);
            }
            catch (RuntimeException err)
            {
                if (statement.CatchBody != null)
                {
                    // Check if it defines a var
                    if (statement.IdentifierToCreate != null)
                    {
                        Environment scope = new Environment(environment);

                        scope.DeclareVariable(
                            ((Identifier)statement.IdentifierToCreate).Symbol, 
                            Values.Helpers.Helpers.CreateObject(new
                            {
                                fullMessage = Values.Helpers.Helpers.CreateString(err.Message),
                            }),
                            new VariableSettings()
                            {
                                IsConstant = true
                            }, 
                            statement.IdentifierToCreate
                        );

                        return Interpreter.Evaluate(statement.CatchBody, scope);
                    } 
                    else return Interpreter.Evaluate(statement.CatchBody, environment);
                }
            }

            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
