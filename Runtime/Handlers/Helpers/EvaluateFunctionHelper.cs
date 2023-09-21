using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue EvaluateFunctionHelper(FunctionValue f, List<RuntimeValue> args, Environment environment, Expression? from = null)
        {
            FunctionValue function = f;

            if (function.Type == Values.ValueType.Function)
            {
                Verbose.Log("Executing Zephyr function " + function.Name);

                // Create scope
                Environment scope = new(function.DeclarationEnvironment);
                scope.DeclareVariable("~return", Values.Helpers.Helpers.CreateNull(), new VariableSettings());

                // Create the argument variables
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    scope.DeclareVariable(function.Parameters[i], args.ElementAtOrDefault(i) == null ? Values.Helpers.Helpers.CreateNull() : args.ElementAt(i), new VariableSettings());
                }

                // Run body
                Interpreter.Evaluate(function.Body, scope);

                var v = scope.LookupVariable("~return");
                v.IsReturn = false;
                return v;
            } else
            {
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = $"Cannot run {function.Type} as a function"
                });
            }
        }

        public static RuntimeValue EvaluateFunctionHelper(NativeFunction func, List<RuntimeValue> args, Environment environment, Expression? from = null)
        {
            if (func.Options != null)
            {
                // Check permissions
                if (func.Options.PermissionsNeeded != "")
                {
                    foreach (char i in func.Options?.PermissionsNeeded)
                    {
                        if (!Program.Options.FileAccessFlags.Contains(i))
                        {
                            throw new RuntimeException(new()
                            {
                                Location = Helpers.GetLocation(from?.Location, func.Location),
                                Error = $"Program not started with file access for {i}",
                                Reference = func
                            });
                        }
                    }
                }

                if (func.Options.UncheckedParameters != true && func.Options.Parameters != null)
                {
                    // Check parameters - amount
                    if (func.Options.Parameters.Count != args.Count)
                    {
                        throw new RuntimeException_new()
                        {
                            Location = GetLocation(from?.Location, func.Location),
                            Error = $"Function {func.Name} expects {func.Options.Parameters.Count} paremters but recieved {args.Count}",
                            Reference = func
                        };
                    }

                    // Check parameters - types
                    for (int i = 0; i < func.Options.Parameters.Count; i++)
                    {
                        if (args[i].Type != func.Options.Parameters[i].Type && func.Options.Parameters[i].Type != Values.ValueType.Any)
                        {
                            throw new RuntimeException(new()
                            {
                                Location = GetLocation(args[i].Location, from?.Location, func.Location),
                                Error = $"Parameter {func.Options.Parameters[i].Name} requires type {func.Options.Parameters[i].Type} but recieved {args[i].Type}",
                                Reference = func
                            });
                        }
                    }
                } 
                
                // Check for all types
                if (func.Options.AllParamsOfType != null)
                {
                    for (int i = 0; i < args.Count; i++)
                    {
                        if (args[i].Type != func.Options.AllParamsOfType && func.Options.Parameters[i].Type != Values.ValueType.Any)
                        {
                            throw new RuntimeException(new()
                            {
                                Location = GetLocation(args[i].Location, from?.Location, func.Location),
                                Error = $"All parameters must be of type {func.Options.AllParamsOfType}, but parameter {i + 1} got type {args[i].Type}",
                                Reference = func
                            });
                        }
                    }
                }
            }

            return func.Call(args, environment, from);
        }
    }
}
