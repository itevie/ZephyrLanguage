using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ZephyrNew.ZephyrCommandLine
{
    [Verb("run", isDefault: true, HelpText = $"Run a Zephyr file")]
    internal class Run
    {
        [Option('f', "file", HelpText = $"The Zephyr code file to execute")]
        public string? FileNameFlag { get; set; } = null;

        [Option("debug", HelpText = $"Whether or not to show debug text")]
        public bool Debug { get; set; } = false;
    }
}
