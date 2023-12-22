using Pastel;
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
        public static Package Thread = new Package("Thread", new Values.ObjectValue(new
        {
            createThread = new Values.NativeFunction((options) =>
            {
                FunctionValue func = ((FunctionValue)options.Arguments[0]);
                Stack thisStack = StackContainer.GetStack();

                Thread thread = new Thread(new ThreadStart(new Action(() =>
                {
                    try
                    {
                        lock (StackContainer.Stacks)
                        {
                            StackContainer.CreateStack(thisStack);
                        }
                        Handlers.Helpers.ExecuteZephyrFunction(func, new List<RuntimeValue>(), options.Environment);
                    }
                    catch (RuntimeException e)
                    {
                        Console.WriteLine($"Unrecoverable exception thrown in thread".Pastel(ConsoleColor.Red));
                        Console.WriteLine(e.Visualise());
                        System.Environment.Exit(1);
                    }
                })));

                return new ObjectValue(new
                {
                    start = new NativeFunction((op) =>
                    {
                        thread.Start();
                        return new NullValue();
                    }, new Options("start")),
                });
            }, new Options("setInterval", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Function), "cb"),
            })),

            sleep = new NativeFunction((options) =>
            {
                int time = (int)((NumberValue)options.Arguments[0]).Value;

                try
                {
                    System.Threading.Thread.Sleep(time);
                } catch (Exception e)
                {
                    throw new RuntimeException($"{e.Message}", options.Location);
                }

                return new NullValue();
            }, new Options("sleep", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Number), "ms"),
            }))
        }));
    }
}
