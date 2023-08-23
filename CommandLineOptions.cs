using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    [Verb("run", isDefault: true, HelpText = "Run a Zephyr file")]
    internal class CommandLineOptions
    {
        [Option('f', "file", HelpText = "The Zephyr code file to execute")]
        public string? FileNameFlag { get; set; } = null;

        [Value(0, HelpText = "The Zephyr code file to execute")]
        public string? FileName { get; set; } = null;

        [Option('p', "pipe", HelpText = "The file path to pipe the output to")]
        public string? Pipe { get; set; } = null;

        [Option('c', "config", HelpText = "The config file for the application, this can contain all permissions etc")]
        public string? Config { get; set; } = null;

        [Option("max-runtime", HelpText = "The amount of seconds the program is allowed to execute for")]
        public int? MaxRuntime { get; set; } = null;

        [Option("max-iterations", HelpText = "The max amount of iterations a loop is allowed to make", Default = 10000)]
        public int MaxLoopIterations { get; set; } = 10000;

        [Option("no-iteration-limit", HelpText = "Whether or not the loop iteration limit should be disabled", Default = false)]
        public bool NoIterationLimit { get; set; } = false;

        [Option('a', "file-access", HelpText = "The file permissions the program has access to.\nr = read, w = write, d = delete file, x = delete directory, c = create, n = none\nExample: --file-access=rwdc, -a=n", Default = "rwdxc")]
        public string FileAccessFlags { get; set; } = "rwdxc";

        [Option("system-details", HelpText = "Whether or not the program has access to system details, e.g. username, operating system etc.", Default = true)]
        public bool CanAccessSystemDetails { get; set; } = true;

        [Option("debug", HelpText = "Whether or not the interpreter should produce debug information", Default = false)]
        public bool Debug { get; set; } = false;

        [Option("verbose", HelpText = "Whether or not the interpreter should produce verbose log information", Default = false)]
        public bool Verbose { get; set; } = false;

        [Option("os-apis", HelpText = "Whether or not the program can access OS api's such as user32.dll", Default = true)]
        public bool CanUseSystemAPIs { get; set; } = true;

        [Option("can-spawn-processes", HelpText = "Whether or not the program can spawn processes\nDANGEROUS: This will mean it can execute ANY operation on the system", Default = true)]
        public bool CanSpawnProcesses { get; set; } = true;

        // This is just here for visual
        [Option("args", HelpText = "Everything after will count as the args for the Zephyr application")]
        public string? Args { get; set; } = null;
    }

    [Verb("install-package", HelpText = "Install a Zephyr package from a repository")]
    internal class CommandLineOptionsInstallPackage
    {
        [Value(0, HelpText = "The package to install", Required = true)]
        public string PackageName { get; set; } = "";

        [Value(0, HelpText = "The version of the package", Default = "@latest")]
        public string PackageVersion { get; set; } = "@latest";

        [Option('r', "repository", HelpText = "The repository URL from which to download the packages", Default = "http://localhost:3000")]
        public string RepositoryUrl { get; set; } = "";
    }

    [Verb("new", HelpText = "Initiate a new Zephyr project / package")]
    internal class CommandLineOptionsCreatePackage
    {

    }
}
