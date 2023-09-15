using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package EventPkg = new Package("Event", new
        {
            subscribe = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ((Values.EventValue)args[0]).AddListener((FunctionValue)args[1]);
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "subscribe",
                Parameters =
                {
                    new()
                    {
                        Name = "event",
                        Type = Values.ValueType.Event
                    },
                    new()
                    {
                        Name = "function",
                        Type = Values.ValueType.Function
                    }
                }
            }),

            unsubscribe = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ((Values.EventValue)args[0]).RemoveListener((FunctionValue)args[1]);
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "unsubscribe",
                Parameters =
                {
                    new()
                    {
                        Name = "event",
                        Type = Values.ValueType.Event
                    },
                    new()
                    {
                        Name = "function",
                        Type = Values.ValueType.Function
                    }
                }
            }),

            call = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                EventValue ev = (EventValue)args[0];
                RuntimeValue val = (RuntimeValue)args[1];

                // Check type
                if (val.Type != ev.EventType && ev.EventType != Values.ValueType.Any)
                {
                    throw new RuntimeException(new()
                    {
                        Error = $"Event type is {ev.EventType} but it was called with a {val.Type}",
                        Location = expr.Location
                    });
                }

                ev.ExecuteListeners(val, env);
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "call",
                Parameters =
                {
                    new()
                    {
                        Name = "event",
                        Type = Values.ValueType.Event
                    },
                    new()
                    {
                        Name = "value",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            removeAllListeners = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ((Values.EventValue)args[0]).Listeners.Clear();
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "removeAllListeners",
                Parameters =
                {
                    new()
                    {
                        Name = "event",
                        Type = Values.ValueType.Event
                    }
                }
            }),

            getListeners = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                ArrayValue val = new ArrayValue();

                foreach (FunctionValue func in ((EventValue)args[0]).Listeners)
                {
                    val.Items.Add(func);
                }

                return val;
            }, options: new()
            {
                Name = "getListeners",
                Parameters =
                {
                    new()
                    {
                        Name = "event",
                        Type = Values.ValueType.Event
                    }
                }
            }),
        });
    }
}
