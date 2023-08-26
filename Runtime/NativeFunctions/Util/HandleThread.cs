using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static void HandleThread(Action action)
        {
            Action ac = new(() =>
            {
                try
                {
                    action();
                } catch (Exception err)
                {
                    Console.WriteLine(err.Message + "\nUnrecoverable exception thrown in thread".Pastel(ConsoleColor.Red));
                    System.Environment.Exit(0);
                }
            });

            Thread thread = new(new ThreadStart(ac));
            thread.Start();
        }
    }
}
