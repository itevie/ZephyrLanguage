using System.Globalization;
using System.Text;
using Zephyr.Runtime;
using Zephyr.Runtime.Values;

namespace Zephyr.Runner
{
    internal class Repl
    {
        Runtime.Environment environment = new();

        public void Start()
        {
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
