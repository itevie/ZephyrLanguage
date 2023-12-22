using Pastel;
using System.Diagnostics;
using ZephyrNew.Runner;
using ZephyrNew.Runtime;
using CommandLine;

namespace ZephyrNew
{
    internal class Program
    {
        public static ZephyrCommandLine.Run Options = new ZephyrCommandLine.Run();

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<ZephyrCommandLine.Run>(args)
                .WithParsed<ZephyrCommandLine.Run>(options =>
                {
                    Options = options;
                    string? fileName = options.FileNameFlag ?? null;

                    // Check if it is null
                    if (fileName == null)
                    {
                        // Start repl
                        new Repl().Start();
                        return;
                    }

                    // Execute file
                    Runner.FileExecutor.ExecuteFile(fileName);
                });
        }
    }
}