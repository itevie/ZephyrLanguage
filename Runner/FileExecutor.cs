using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime;
using Pastel;
using Zephyr.Lexer;

namespace Zephyr.Runner
{
    /// <summary>
    /// This is the type of runner that executes once a file name is provided
    /// </summary>
    internal class FileExecutor
    {
        public static Runtime.Environment GlobalEnvironment = new();

        public static void Execute(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new RunnerException($"Cannot find the file: {fileName}");
            }

            // Read file
            string sourceCode = File.ReadAllText(fileName);

            // Generate env
            Runtime.Environment env = new(GlobalEnvironment);
            env.Directory = Program.EntryPoint;

            try
            {
                // Produce AST and execute
                Parser.AST.Program program = new Parser.Parser().ProduceAST(sourceCode, fileName);
                RuntimeValue value = Interpreter.Evaluate(program, env);
            }
            catch (ZephyrException e)
            {
                Console.WriteLine(e.Message);
                Program.PipeOutput += e.Message + "\n";
            } catch (ZephyrException_new e)
            {
                Console.WriteLine(e.Visualise());
            }
        }
    }
}
