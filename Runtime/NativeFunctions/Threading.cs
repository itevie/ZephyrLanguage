using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        /// <summary>
        /// Main threading package - this package is NOT reliable.
        /// Working on how threads work in both C# and in the language is needed.
        /// </summary>
        public static Package Threading = new("Threading", new
        {
            sleep = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Int });
                int time = (int)((IntegerValue)args[0]).Value;

                Debug.Log($"Thread sleeping for {time}");
                System.Threading.Thread.Sleep(time);

                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "sleep",
                Parameters =
                {
                    new()
                    {
                        Name = "milliseconds",
                        Type = Values.ValueType.Int
                    },
                },
                Description = "Pauses the current thread for X amount of milliseconds"
            }),

            threads = new
            {
                create = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Util.ExpectExact(args, new() { Values.ValueType.Function });
                    return GenerateThreadObject((FunctionValue)args[0], env, expr);
                }, options: new()
                {
                    Name = "create",
                    Parameters =
                    {
                        new()
                        {
                            Name = "threadsFunction",
                            Type = Values.ValueType.Function
                        },
                    },
                    Description = "Creates a new thread object which can then be started"
                }),

                waitAll = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    List<Task> tasks = new();
                    Dictionary<int, RuntimeValue> returns = new();
                    List<Exception> errors = new();

                    string type = $"threading.threads.waitAll - {Guid.NewGuid()}";

                    int idx = 0;

                    // Loop through args
                    foreach (RuntimeValue val in args)
                    {
                        int cidx = idx;

                        // Check types
                        if (val.Type != Values.ValueType.Function)
                        {
                            throw new RuntimeException(new()
                            {
                                Location = Handlers.Helpers.GetLocation(val.Location, expr.Location, expr.FullExpressionLocation),
                                Error = "Expected function"
                            });
                        }

                        Debug.Log($"Thread {cidx} function name: {((FunctionValue)val).Name}", type);

                        // Add task
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            Debug.Log($"Thread {cidx} started", type);
                            try
                            {
                                RuntimeValue v = Handlers.Helpers.EvaluateFunctionHelper((FunctionValue)val, new List<RuntimeValue>(), env, expr);
                                Debug.Log($"Thread {cidx} returned a {v.Type}", type);
                                returns.Add(cidx, v);
                            } catch (Exception e)
                            {
                                Console.WriteLine(e.Message + "\nUnrecoverable exception thrown in thread");
                                System.Environment.Exit(0);
                            }
                            Debug.Log($"Thread {cidx} finished", type);
                        }));

                        idx++;
                    }
                       
                    // Wait for tasks
                    Task.WaitAll(tasks.ToArray());

                    List<RuntimeValue> values = new();

                    Debug.Log($"Combining thread's returned values ({returns.Count})", type);

                    // Create array of values
                    foreach (KeyValuePair<int, RuntimeValue> r in returns.OrderBy(i => i.Key))
                    {
                        values.Add(r.Value);
                    }

                    return Helpers.CreateArray(values);
                }, options: new()
                {
                    Name = "waitAll",
                    UncheckedParameters = true,
                    AllParamsOfType = Values.ValueType.Function,
                    Description = "Executes every function in the parameters as a new thread simultaneously, waits for execution and returns the output as an array"
                }),
            }
        });

        /// <summary>
        /// This generates a new thread object - basically creating a new instance of a class, but it returns an object.
        /// </summary>
        /// <param name="function">The function to run</param>
        /// <param name="environment">The environment to run it in</param>
        /// <param name="expression">The expression containing location details</param>
        /// <returns>The obejct that was created</returns>
        public static ObjectValue GenerateThreadObject(FunctionValue function, Environment environment, Parser.AST.Expression expression)
        {
            return Helpers.CreateObject(new
            {
                start = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    string type = $"threading.threads.create.start - {Guid.NewGuid()}";

                    Debug.Log($"Thread function name: {function.Name}", type);

                    void f()
                    {
                        try
                        {
                            Debug.Log($"Thread started", type);
                            Runtime.Handlers.Helpers.EvaluateFunctionHelper(function, new List<RuntimeValue>(), environment, expression);
                            Debug.Log($"Thread finished", type);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.Message + "\nUnrecoverable exception thrown in thread");
                            System.Environment.Exit(0);
                        }
                    }

                    Thread thread = new(new ThreadStart((Action)f));
                    thread.Start();

                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "start",
                }),
            });
        }
    }
}
