using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        /// <summary>
        /// This package is used for executing Zephyr code directly in the language.
        /// Similar to JavaScript's eval() function, but instead it is creating an entire new environemnt.
        /// </summary>
        public static Package EnvironmentEvalPkg = new("Evaluator", new
        {
            createEnvironment = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return EnvironmentEvalCreater();
            }, options: new()
            {
                Name = "create"
            })
        });

        public static ObjectValue EnvironmentEvalCreater()
        {
            Environment environment = new();

            return Helpers.CreateObject(new
            {
                evaluate = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    try
                    {
                        // Produce AST and execute
                        Parser.AST.Program program = new Parser.Parser().ProduceAST(((StringValue)args[0]).Value, "eval");
                        RuntimeValue value = Interpreter.Evaluate(program, environment);
                        return value;
                    }
                    catch (ParserException e)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                            Error = $"Parser error: {e.Message}"
                        });
                    }
                    catch (RuntimeException e)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                            Error = $"Runtime error: {e.Message}"
                        });
                    }
                    catch (LexerException e)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                            Error = $"Lexer error: {e.Message}"
                        });
                    }
                }, options: new()
                {
                    Name = "create",
                    Parameters =
                    {
                        new()
                        {
                            Name = "evaluationString",
                            Type = Values.ValueType.String
                        }
                    }
                })
            });
        }
    }
}
