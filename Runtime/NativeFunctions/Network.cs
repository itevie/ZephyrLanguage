using System.Runtime.CompilerServices;
using Websocket.Client;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;
using ValueType = Zephyr.Runtime.Values.ValueType;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package NetPkg = new("Net", new
        {
            ws = Helpers.CreateObject(new
            {
                createWebsocket = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    return CreateWSObject(new Uri(((StringValue)args[0]).Value), env);
                }, options: new()
                {
                    Name = "createWebsocket",
                    Parameters =
                    {
                        new()
                        {
                            Name = "URL",
                            Type = Values.ValueType.String
                        }
                    }
                }),
            }),

            http = new
            {
                createClient = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    return CreateHTTPObject(env);
                }, options: new()
                {
                    Name = "createClient"
                }),

                createServer = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    string url = ((StringValue)args[0]).Value;
                    FunctionValue func = ((FunctionValue)args[1]);
                    return HttpServerCreator(url, func, env);
                }, options: new()
                {
                    Name = "createServer",
                    Parameters =
                    {
                        new()
                        {
                            Name = "url",
                            Type = Values.ValueType.String
                        },
                        new()
                        {
                            Name = "callback",
                            Type = ValueType.Function
                        }
                    }
                }),
            }
        });

        public static ObjectValue CreateHTTPObject(Environment environment)
        {
            HttpClient httpClient = new();

            return Helpers.CreateObject(new
            {
                setHeader = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    httpClient.DefaultRequestHeaders.Add(((StringValue)args[0]).Value, ((StringValue)args[1]).Value);
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "setHeader",
                    Parameters =
                    {
                        new()
                        {
                            Name = "headerKey",
                            Type = Values.ValueType.String
                        },
                        new()
                        {
                            Name = "headerValue",
                            Type = Values.ValueType.String
                        }
                    }
                }),

                get = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Uri url = new(((StringValue)args[0]).Value);

                    Debug.Log($"GETting {url}");
                    HttpResponseMessage result = httpClient.GetAsync(url).GetAwaiter().GetResult();
                    Debug.Log($"HTTP request to {url} returned {result.StatusCode} {result.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                    return Helpers.CreateObject(new
                    {
                        statusCode = (int)result.StatusCode,
                        content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    });
                }, options: new()
                {
                    Name = "post",
                    Parameters =
                    {
                        new()
                        {
                            Name = "url",
                            Type = Values.ValueType.String
                        },
                    }
                }),

                post = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Uri url = new(((StringValue)args[0]).Value);
                    string data = new(((StringValue)args[1]).Value.Where(c => !char.IsControl(c)).ToArray());
                    string mediaType = ((StringValue)args[2]).Value;
                    Debug.Log($"Sending {data} to {url}");
                    HttpResponseMessage result = httpClient.PostAsync(url, new StringContent(data, System.Text.Encoding.UTF8, mediaType)).GetAwaiter().GetResult();
                    Debug.Log($"HTTP request to {url} returned {result.StatusCode} {result.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                    return Helpers.CreateObject(new
                    {
                        statusCode = (int)result.StatusCode,
                        content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                    });
                }, options: new()
                {
                    Name = "post",
                    Parameters =
                    {
                        new()
                        {
                            Name = "url",
                            Type = Values.ValueType.String
                        },
                        new()
                        {
                            Name = "stringData",
                            Type = ValueType.String
                        },
                        new()
                        {
                            Name = "dataEncoding",
                            Type = ValueType.String
                        }
                    }
                }),
            });
        } 

        public static ObjectValue CreateWSObject(Uri uri, Environment environment)
        {
            ManualResetEvent exitEvent = new(false);
            WebsocketClient client = new(uri);

            List<FunctionValue> messageEventCallbacks = new();
            List<FunctionValue> disconnectionEventCallbacks = new();
            List<FunctionValue> errorEventCallbacks = new();

            Action<string> throwError = (string message) =>
            {
                Debug.Log($"Throwing error in WS");
                try
                {
                    foreach (FunctionValue func in errorEventCallbacks)
                    {
                        Handlers.Helpers.EvaluateFunctionHelper(func, new() { Helpers.CreateString(message) }, environment);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"WS exception occurred: {e.Message}");
                    Console.WriteLine(e.Message + "\nUnrecoverable exception thrown in WS thread: " + message);
                    System.Environment.Exit(0);
                }
            };

            // Listen for events
            client.DisconnectionHappened.Subscribe(msg =>
            {
                try
                {
                    Debug.Log($"WS disconnect: {msg.Exception?.Message} {msg.CloseStatusDescription}");

                    string closeReason = $"{msg.Exception?.Message ?? msg.CloseStatusDescription}";
                    if (closeReason == "")
                    {
                        closeReason = "Unknown closure reason";
                    }

                    foreach (FunctionValue func in disconnectionEventCallbacks)
                    {
                        Handlers.Helpers.EvaluateFunctionHelper(func, new() { Helpers.CreateObject(new
                        {
                            message = Helpers.CreateString(closeReason)
                        }) }, environment);
                    }
                } catch (Exception e)
                {
                    if (Program.Options.Debug)
                    {
                        Console.WriteLine(e);
                    }

                    Debug.Log($"Exception occurred in WS thread: {e.Message}");
                    throwError($"{e.Message}");
                }
            });

            client.MessageReceived.Subscribe(msg =>
            {
                try
                {
                    Debug.Log($"WS message: {msg}");
                    foreach (FunctionValue func in messageEventCallbacks)
                    {
                        Helpers.CreateObject(new
                        {
                            message = ""
                        });
                        Handlers.Helpers.EvaluateFunctionHelper(func, new() { Helpers.CreateString(msg.Text) }, environment);
                    }
                } catch (Exception e)
                {
                    if (Program.Options.Debug)
                    {
                        Console.WriteLine(e);
                    }

                    Debug.Log($"Exception occurred in WS thread: {e.Message}");
                    throwError($"{e.Message}");
                }
            });

            return Helpers.CreateObject(new
            {
                events = Helpers.CreateObject(new
                {
                    message = new
                    {
                        subscribe = Helpers.CreateNativeFunction((args, env, expr) =>
                        {
                            messageEventCallbacks.Add(((FunctionValue)args[0]));
                            return Helpers.CreateNull();
                        }, options: new()
                        {
                            Name = "subscribe",
                            Parameters =
                            {
                                new()
                                {
                                    Name = "callback",
                                    Type = Values.ValueType.Function
                                }
                            }
                        }),
                    },

                    disconnect = new
                    {
                        subscribe = Helpers.CreateNativeFunction((args, env, expr) =>
                        {
                            disconnectionEventCallbacks.Add((FunctionValue)args[0]);
                            return Helpers.CreateNull();
                        }, options: new()
                        {
                            Name = "subscribe",
                            Parameters =
                            {
                                new()
                                {
                                    Name = "callback",
                                    Type = Values.ValueType.Function
                                }
                            }
                        }),
                    },

                    error = new
                    {
                        subscribe = Helpers.CreateNativeFunction((args, env, expr) =>
                        {
                            errorEventCallbacks.Add((FunctionValue)args[0]);
                            return Helpers.CreateNull();
                        }, options: new()
                        {
                            Name = "subscribe",
                            Parameters =
                            {
                                new()
                                {
                                    Name = "callback",
                                    Type = Values.ValueType.Function
                                }
                            }
                        }),
                    }
                }),

                setReconnectionTimeout = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    client.ReconnectTimeout = TimeSpan.FromSeconds(((IntegerValue)args[0]).Value);
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "setReconnectionTimeout",
                    Parameters =
                    {
                        new()
                        {
                            Name = "seconds",
                            Type = Values.ValueType.Int
                        }
                    }
                }),

                connect = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Debug.Log("Connecting to WS...");
                    client.Start();
                    exitEvent.WaitOne();
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "connect",
                }),

                send = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    Debug.Log($"WS Send: {((StringValue)args[0]).Value}");
                    client.Send(((StringValue)args[0]).Value);
                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "send",
                    Parameters =
                    {
                        new()
                        {
                            Name = "stringData",
                            Type = Values.ValueType.String
                        }
                    }
                }),
            });
        }
    }
}
