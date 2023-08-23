using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class Debug
    {
        public static void Log(string message, string type = "")
        {
            if (Program.Options.Debug || Program.Options.Verbose)
                Console.WriteLine($"[Debug:{type}] {message}".Pastel(ConsoleColor.Gray));
        }

        public static void Error(string message, string type = "")
        {
            if (Program.Options.Debug || Program.Options.Verbose)
                Console.WriteLine($"[Debug:{type}] {message}".Pastel(ConsoleColor.Red).PastelBg(ConsoleColor.Gray));
        }

        public static void Warning(string message, string type = "")
        {
            if (Program.Options.Debug || Program.Options.Verbose)
                Console.WriteLine($"[Warning:{type}] {message}".Pastel(ConsoleColor.Yellow).PastelBg(ConsoleColor.Gray));
        }
    }

    internal class Verbose
    {
        public static void Log(string message, string type = "")
        {
            if (Program.Options.Verbose)
                Console.WriteLine($"[Verbose:{type}] {message}".Pastel(ConsoleColor.DarkCyan));
        }

        public static void Error(string message, string type = "")
        {

            if (Program.Options.Verbose)
                Console.WriteLine($"[Verbose:{type}] {message}".Pastel(ConsoleColor.DarkRed).PastelBg(ConsoleColor.DarkCyan));
        }
    }
}
