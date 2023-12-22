using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package ConsolePackage = new Package("Console", new Values.ObjectValue(new
        {
            writeLine = new NativeFunction((ExecutorOptions options) =>
            {
                Console.WriteLine(options.Arguments[0].Visualise());
                return new NullValue();
            }, new Options("writeLine", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Any), "toLog"),
            })),

            readLine = new NativeFunction((ExecutorOptions options) =>
            {
                return new StringValue(Console.ReadLine() ?? "");
            }, new Options("readLine", new List<Parameter>())),

            clear = new NativeFunction((ExecutorOptions options) =>
            {
                Console.Clear();
                return new NullValue();
            }, new Options("clear", new List<Parameter>())),

            style = new
            {
                reset = "\x1b[0m",
                bright = "\x1b[1m",
                dim = "\x1b[2m",
                underscore = "\x1b[4m",
                blink = "\x1b[5m",
                reverse = "\x1b[7m",
                hidden = "\x1b[8m",

                fgBlack = "\x1b[30m",
                fgRed = "\x1b[31m",
                fgGreen = "\x1b[32m",
                fgYellow = "\x1b[33m",
                fgBlue = "\x1b[34m",
                fgMagenta = "\x1b[35m",
                fgCyan = "\x1b[36m",
                fgWhite = "\x1b[37m",
                fgGray = "\x1b[90m",

                bgBlack = "\x1b[40m",
                bgRed = "\x1b[41m",
                bgGreen = "\x1b[42m",
                bgYellow = "\x1b[43m",
                bgBlue = "\x1b[44m",
                bgMagenta = "\x1b[45m",
                bgCyan = "\x1b[46m",
                bgWhite = "\x1b[47m",
                bgGray = "\x1b[100m",
            }
        }));
    }
}
