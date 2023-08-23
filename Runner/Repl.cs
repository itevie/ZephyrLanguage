using Zephyr.Runtime;
using Zephyr.Runtime.Values;

namespace Zephyr.Runner
{
    internal class Repl
    {
        Runtime.Environment environment = new Runtime.Environment();

        public void Start()
        {
            environment.Directory = Program.EntryPoint;
            while (true)
            {
                try
                {
                    // Get input - read
                    Console.Write("> ");
                    string? input = Console.ReadLine();

                    // Check if ends with semi
                    if (input?.EndsWith(";") == false)
                    {
                        input += ";";
                    }
                    
                    if (input !=  null)
                    {
                        // Get value - eval
                        Parser.AST.Program program = new Parser.Parser().ProduceAST(input, "REPL");
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
    }
}
