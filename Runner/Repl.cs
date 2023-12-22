using ZephyrNew.Lexer;
using ZephyrNew.Runtime;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runner
{
    internal class Repl
    {
        private Runtime.Environment environment = new Runtime.Environment(System.Environment.CurrentDirectory);

        public void Start()
        {
            StackContainer.CreateStack();
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (input == null) continue;

                // Check if semicolon is missing
                if (!input.EndsWith(";"))
                    input += ";";

                try
                {
                    Parser.AST.Program astTree = new Parser.Parser().ProduceAST(input, "<REPL>");
                    RuntimeValue result = Interpreter.Evaluate(astTree, environment);

                    Console.WriteLine(result.Visualise());
                } catch (ZephyrException e)
                {
                    Console.WriteLine(e.Visualise());
                }
            }
        }
    }
}
