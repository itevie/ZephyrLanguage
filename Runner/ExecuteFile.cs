using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime;

namespace ZephyrNew.Runner
{
    internal class FileExecutor
    {
        public static void ExecuteFile(string fileName)
        {
            StackContainer.CreateStack();
            // Check if file exists
            string pathName = ZephyrPath.Resolve(System.Environment.CurrentDirectory, fileName);

            // Check if it exists
            if (!File.Exists(pathName))
            {
                Console.WriteLine($"Cannot find the file: {pathName}".Pastel(ConsoleColor.Red));
                System.Environment.Exit(1);
            }

            string contents = File.ReadAllText(pathName);

            Runtime.Environment environment = new Runtime.Environment(new FileInfo(pathName).DirectoryName);

            try
            {
                Parser.AST.Program program = new Parser.Parser().ProduceAST(contents, pathName);
                Interpreter.Evaluate(program, environment);
            } catch (ZephyrException exception)
            {
                Console.WriteLine(exception.Visualise());
                //System.Environment.Exit(1);
            }
        }
    }
}
