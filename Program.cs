using Zephyr.Parser;
using Zephyr.Lexer;
using Zephyr.Parser.AST;
using Zephyr.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Zephyr.Runtime.Values;
using System.Windows.Markup;
using Zephyr.Runner;
using CommandLine;
using System.Resources;
using Pastel;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using Zephyr.Runtime.NativeFunctions;
using System.Runtime.CompilerServices;
using NetCoreAudio;

namespace Zephyr
{
    internal class Program
    {
        public static CommandLineOptions Options { get; set; } = new CommandLineOptions();
        public static Dictionary<string, PackageManager.ZephyrLoadedPackage> LoadedPackages = new();
        public static string EntryPoint = Directory.GetCurrentDirectory();
        public static List<string> ZephyrArgs = new List<string>();

        public static string PipeOutput { get; set; } = "";

        /// <summary>
        /// Ran once the Zephyr progeam has finished executing  
        /// </summary>
        public static void Finish()
        {
            if (Options.Pipe != null)
                File.WriteAllText(Options.Pipe, PipeOutput);
            System.Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            try
            {
                // Construct new args
                //List<string> zephyrArgs = new();
                List<string> runnerArgs = new();

                bool after = false;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--args" && after != true)
                    {
                        after = true;
                    }
                    else if (after != true)
                    {
                        // test
                        runnerArgs.Add(args[i]);
                    }
                    else
                    {
                        ZephyrArgs.Add(args[i]);
                    }
                }

                CommandLine.Parser.Default.ParseArguments<
                    CommandLineOptions, CommandLineOptionsInstallPackage,
                    CommandLineOptionsCreatePackage, CommandLineOptionsRegisterUser,
                    CommandLineOptionsUploadPackage, CommandLineOptionsDocumentation
                >(runnerArgs.ToArray())
                       .WithParsed<CommandLineOptions>(o =>
                       {
                           Options = o;
                           string? fileName = o?.FileNameFlag ?? o?.FileName ?? null;
                           if (fileName != null)
                           {
                               EntryPoint = new FileInfo(fileName).Directory.FullName;
                           }

                           Verbose.Log($"C# AppContext is {AppContext.BaseDirectory}");

                           // Check for specified CWD
                           if (o.CurrentWorkingDirectory != null)
                           {
                               EntryPoint = Path.GetFullPath(o.CurrentWorkingDirectory);
                           }

                           Debug.Log($"Entry point is {EntryPoint}");

                           PackageManager.PackageLoader.LoadPackages();

                           // Load packages

                           if (fileName == null || fileName == "repl")
                           {
                               // If filename is null, run REPL
                               Console.WriteLine($"Running in REPL mode - type help() for help");

                               Repl repl = new();
                               repl.Start();
                           }
                           else
                           {
                               bool done = false;

                               try
                               {
                                   if (Options.MaxRuntime != null)
                                   {
                                       Thread thread = new(new ThreadStart(new Action(() =>
                                       {
                                           Thread.Sleep((int)Options.MaxRuntime * 1000);

                                           ProcessThreadCollection currentThreads = Process.GetCurrentProcess().Threads;

                                           if (done == false || currentThreads.Count != 1)
                                           {
                                               Exception ex = new ZephyrException(new()
                                               {
                                                   Error = $"The program has exceeded the specified maximum runtime value (--max-runtime={Options.MaxRuntime})"
                                               });
                                               Console.WriteLine(ex.Message);
                                               PipeOutput += ex.Message;
                                               PipeOutput += "\n";
                                               Finish();
                                           }
                                       })));

                                       thread.Start();
                                   }

                                   FileExecutor.Execute(Path.Combine(Directory.GetCurrentDirectory(), fileName));
                               }
                               catch (RunnerException e)
                               {
                                   Console.WriteLine(e.Message.Pastel(ConsoleColor.Red));
                                   PipeOutput += e.Message.Pastel(ConsoleColor.Red) + "\n";
                               }

                               if (Options.Pipe != null)
                               {
                                   Finish();
                               }

                               done = true;
                           }
                       })
                       .WithParsed<CommandLineOptionsCreatePackage>(o =>
                       {
                           PackageManager.ProjectCreator.CreateNewProject();
                       }).WithParsed<CommandLineOptionsInstallPackage>(o =>
                       {
                           PackageManager.RepositoryClient.InstallPackage(o.PackageName, o.PackageVersion, new Uri(o.RepositoryUrl));
                       }).WithParsed<CommandLineOptionsRegisterUser>(o =>
                       {
                           AccountManagement.Register(o.Username, o.Password, o.RepositoryUrl);
                       }).WithParsed<CommandLineOptionsUploadPackage>(o =>
                       {
                           PackageManager.RepositoryClient.UploadPackage(new Uri(o.RepositoryUrl), o.Username, o.Password); 
                       }).WithParsed<CommandLineOptionsDocumentation>(o =>
                       {
                           Console.WriteLine($"The current Zephyr documentation is on GitHub:\nhttps://github.com/itevie/ZephyrLanguage/wiki");
                       });
            } catch (Exception e)
            {
                Console.WriteLine(e);
                PipeOutput += e.Message.Pastel(ConsoleColor.Red) + "\n";
                //Console.WriteLine($"A critical error occurred in the interpreter: {e.Message}".Pastel(ConsoleColor.Red));
                Finish();
            }
        }

        
    }
}