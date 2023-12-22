using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew
{
    internal class Logger
    {
    }

    internal class Debug
    {
        public static void Log(string message, LogType type)
        {
            if (Program.Options.Debug)
                Console.WriteLine($"[Debug:{type}] {message}".Pastel(ConsoleColor.Gray));
        }
    }

    internal enum LogType
    {
        Environment,
        Modules,
        Lexer,
        Stack,
    }
}
