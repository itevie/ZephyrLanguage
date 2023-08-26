using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;
using System.Text.RegularExpressions;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package ConsoleUsage = new("console", new
        {
            clear = Helpers.CreateNativeFunction((args, environment, expr) =>
            {
                Console.Clear();    
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "clear",
            }),

            visualize = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                return Helpers.CreateString(Util.VisualizeType(args[0], noColors: true));
            }, options: new()
            {
                Name = "visualize",
                Parameters =
                {
                    new()
                    {
                        Name = "what",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            @out = new
            {
                writeLine = Helpers.CreateNativeFunction((args, environment, expr) =>
                {
                    string text = "";

                    foreach (RuntimeValue value in args)
                    {
                        text += Util.VisualizeType(value) + " ";
                        Program.PipeOutput += Util.VisualizeType(value) + " ";
                    }
                    Console.WriteLine(text);
                    Program.PipeOutput += "\n";
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "writeLine",
                    UncheckedParameters = true
                }),

                write = Helpers.CreateNativeFunction((args, environment, expr) =>
                {
                    Util.ExpectExact(args, new() { Values.ValueType.Any }, expr);
                    Console.Write(Util.VisualizeType(args[0]));
                    Program.PipeOutput += Util.VisualizeType(args[0]);
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "write",
                    Parameters =
                    {
                        new()
                        {
                            Name = "what",
                            Type = Values.ValueType.String
                        }
                    }
                }),
            },

            @in = new
            {
                readLine = Helpers.CreateNativeFunction((args, environment, expr) =>
                {
                    return Helpers.CreateString(Console.ReadLine() ?? "");
                }, options: new()
                {
                    Name = "readLine"
                }),

                readKey = Helpers.CreateNativeFunction((args, environment, expr) =>
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    RuntimeValue obj = Helpers.CreateObject(new
                    {
                        @char = Helpers.CreateString(key.KeyChar.ToString()),
                        isAlt = Helpers.CreateBoolean(key.Modifiers.HasFlag(ConsoleModifiers.Alt)),
                        isControl = Helpers.CreateBoolean(key.Modifiers.HasFlag(ConsoleModifiers.Control)),
                        isShift = Helpers.CreateBoolean(key.Modifiers.HasFlag(ConsoleModifiers.Shift)),
                    });

                    return obj;
                }, options: new()
                {
                    Name = "readKey"
                }),
            }
        });
    }
}
