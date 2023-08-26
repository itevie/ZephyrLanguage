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

namespace Zephyr
{
    internal class Program
    {
        public static CommandLineOptions Options { get; set; } = new CommandLineOptions();
        public static Dictionary<string, ZephyrLoadedPackage> LoadedPackages = new();
        public static string EntryPoint = Directory.GetCurrentDirectory();

        public static string PipeOutput { get; set; } = "";

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
                List<string> zephyrArgs = new();
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
                        zephyrArgs.Add(args[i]);
                    }
                }

                CommandLine.Parser.Default.ParseArguments<
                    CommandLineOptions, CommandLineOptionsInstallPackage,
                    CommandLineOptionsCreatePackage, CommandLineOptionsRegisterUser,
                    CommandLineOptionsUploadPackage
                >(runnerArgs.ToArray())
                       .WithParsed<CommandLineOptions>(o =>
                       {
                           Options = o;
                           string? fileName = o?.FileName == null ? o?.FileNameFlag : o?.FileName;
                           if (fileName != null)
                           {
                               EntryPoint = new FileInfo(fileName).Directory.FullName;
                           }

                           Debug.Log($"Entry point is {EntryPoint}");

                           LoadPackages();

                           // Load packages

                           if (fileName == null || fileName == "repl")
                           {
                               // If filename is null, run REPL
                               Console.WriteLine($"Running in REPL mode");

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
                       .WithParsed<CommandLineOptionsInstallPackage>(o =>
                       {
                           Console.WriteLine($"Install {o?.PackageName}");
                       })
                       .WithParsed<CommandLineOptionsCreatePackage>(o =>
                       {
                           ProjectCreator.CreateNewProject();
                       }).WithParsed<CommandLineOptionsInstallPackage>(o =>
                       {
                           PackageManager.InstallPackage(o.PackageName, o.PackageVersion, new Uri(o.RepositoryUrl));
                       }).WithParsed<CommandLineOptionsRegisterUser>(o =>
                       {
                           AccountManagement.Register(o.Username, o.Password, o.RepositoryUrl);
                       }).WithParsed<CommandLineOptionsUploadPackage>(o =>
                       {
                           PackageManager.UploadPackage(new Uri(o.RepositoryUrl), o.Username, o.Password); 
                       });
            } catch (Exception e)
            {
                Console.WriteLine(e);
                PipeOutput += e.Message.Pastel(ConsoleColor.Red) + "\n";
                //Console.WriteLine($"A critical error occurred in the interpreter: {e.Message}".Pastel(ConsoleColor.Red));
                Finish();
            }
        }

        static void LoadPackages()
        {
            // Check for package.json
            if (File.Exists(Path.Join(EntryPoint, "package.json")))
            {
                ZephyrPackage? package = JsonConvert.DeserializeObject<ZephyrPackage>(File.ReadAllText(Path.Combine(EntryPoint, "package.json")));

                if (package != null)
                {
                    Debug.Log($"Loading package references...");

                    // Check for zephyr_packages folder
                    if (!Directory.Exists(Path.Combine(EntryPoint, "zephyr_packages")))
                    {
                        Debug.Warning($"Cannot load packages as there is no zephyr_packages folder ({Path.Combine(EntryPoint, "zephyr_packages")})", "load-package-references");
                        return;
                    }

                    // Read all dependencies
                    foreach (KeyValuePair<string, ZephyrPackageDependency> dep in package.Dependencies)
                    {
                        Debug.Log($"Loading package {dep.Key}...");

                        ZephyrLoadedPackage? loadedPackage = null;

                        // Find package
                        foreach (string dir in Directory.EnumerateDirectories(Path.Combine(EntryPoint, "zephyr_packages")))
                        {
                            // Try read package.json
                            if (File.Exists(Path.Combine(dir, "package.json")))
                            {
                                // Read it
                                ZephyrPackage? pkg = JsonConvert.DeserializeObject<ZephyrPackage>(File.ReadAllText(Path.Combine(dir, "package.json")));

                                if (pkg != null && pkg.Name == dep.Key)
                                {
                                    // Check for entry point
                                    if (pkg.EntryPoint != "")
                                    {
                                        // Check if entry point exists
                                        if (File.Exists(Path.Combine(dir, pkg.EntryPoint)))
                                        {
                                            // Load it
                                            loadedPackage = new ZephyrLoadedPackage(Path.Combine(dir, pkg.EntryPoint));
                                        }
                                    }
                                }
                            }
                        }

                        // Check if it was null
                        if (loadedPackage == null)
                        {
                            Debug.Warning($"Failed to load package {dep.Key}");
                            continue;
                        }

                        // Add to packages
                        LoadedPackages.Add(dep.Key, loadedPackage);
                        Debug.Log($"Loaded package reference {dep.Key} successfully!");
                    }
                }
            }
        }
    }
}