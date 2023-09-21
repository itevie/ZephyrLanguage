using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Zephyr.Runtime;
using Zephyr.Runtime.Values;

namespace Zephyr.Runner
{
    /// <summary>
    /// This is ran when no file name is provided, works same as any other programming language's REPL mode works.
    /// </summary>
    internal class Repl
    {
        Runtime.Environment environment = new();

        public void SetupEnv()
        {
            void createNew(string name, Func<List<RuntimeValue>, Runtime.Environment, Parser.AST.Expression?, RuntimeValue> f)
            {
                environment.DeclareVariable(name, Runtime.Values.Helpers.Helpers.CreateNativeFunction(f, name = "REPL::" + name), new()
                {
                    IsConstant = true,
                    Modifiers = new()
                    {
                        Modifier.Final
                    },
                    Origin = "REPL",
                });
            }

            createNew("exit", (args, env, expr) =>
            {
                System.Environment.Exit(0);
                return Runtime.Values.Helpers.Helpers.CreateNull();
            });

            createNew("flush", (args, env, expr) =>
            {
                environment = new();
                SetupEnv();
                return Runtime.Values.Helpers.Helpers.CreateNull();
            });

            createNew("help", (args, env, expr) =>
            {
                Console.WriteLine("----- REPL help -----");
                Console.WriteLine(" exit() - Exit repl mode (or CTRL+C)");
                Console.WriteLine(" flush() - Sets up a new environment");
                Console.WriteLine(" help() - Shows this");
                return Runtime.Values.Helpers.Helpers.CreateNull();
            });
        }

        public void Start()
        {
            SetupEnv();
            environment.Directory = Program.EntryPoint;
            while (true)
            {
                try
                {
                    // Get input - read
                    Console.Write("> ");
                    //string? input = Console.ReadLine();

                    /*List<string> data = new();

                    // Fetch auto-completion data
                    foreach (KeyValuePair<string, Variable> item in environment._variables)
                    {
                        data.Add(item.Key);
                    }

                    var builder = new StringBuilder();
                    var input = Console.ReadKey(intercept: true);

                    while (input.Key != ConsoleKey.Enter)
                    {
                        var currentInput = builder.ToString();
                        if (input.Key == ConsoleKey.Tab)
                        {
                            var match = data.FirstOrDefault(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));
                            if (string.IsNullOrEmpty(match))
                            {
                                input = Console.ReadKey(intercept: true);
                                continue;
                            }

                            ClearCurrentLine();
                            builder.Clear();

                            Console.Write("> " + match);
                            builder.Append(match);
                        }
                        else
                        {
                            if (input.Key == ConsoleKey.Backspace && currentInput.Length > 0)
                            {
                                builder.Remove(builder.Length - 1, 1);
                                ClearCurrentLine();

                                currentInput = currentInput.Remove(currentInput.Length - 1);
                                Console.Write("> " + currentInput);
                            }
                            else
                            {
                                var key = input.KeyChar;
                                builder.Append(key);
                                Console.Write(key);
                            }
                        }

                        input = Console.ReadKey(intercept: true);
                    }

                    string finInput = builder.ToString();
                    Console.WriteLine();*/
                    string finInput = Console.ReadLine();

                    if (finInput.EndsWith(";") == false)
                        finInput += ";";
                    
                    if (finInput !=  null)
                    {
                        // Get value - eval
                        Parser.AST.Program program = new Parser.Parser().ProduceAST(finInput, "REPL");
                        RuntimeValue value = Interpreter.Evaluate(program, environment);

                        // Print - print
                        Console.WriteLine(Runtime.NativeFunctions.Util.VisualizeType(value));
                    }
                }
                catch (ZephyrException e)
                {
                    Console.WriteLine(e.Message);
                } catch (ZephyrException_new e)
                {
                    Console.WriteLine(e.Visualise());
                }
            }
        }
        
        /// <remarks>
        /// https://stackoverflow.com/a/8946847/1188513
        /// </remarks>>
        private static void ClearCurrentLine()
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }
    }
}
