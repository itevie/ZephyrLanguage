using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {

        public static ObjectValue HttpServerCreator(string url, FunctionValue func, Environment env)
        {
            HttpListener listener = new();
            listener.Prefixes.Add(url);

            return Helpers.CreateObject(new
            {
                start = Helpers.CreateNativeFunction((args, env, expr) =>
                {
                    HandleThread(new Action(() =>
                    {
                        async Task HandleIncomingConnections()
                        {
                            while (true)
                            {
                                HttpListenerContext ctx = await listener.GetContextAsync();

                                HttpListenerRequest req = ctx.Request;
                                HttpListenerResponse resp = ctx.Response;

                                bool respondedTo = false;

                                Handlers.Helpers.EvaluateFunctionHelper(func, new()
                                {
                                    Helpers.CreateObject(new
                                    {
                                        request = new
                                        {
                                            url = req.Url.ToString(),
                                            method = req.HttpMethod.ToUpper(),
                                            body = new StreamReader(req.InputStream).ReadToEnd()
                                        },
                                        response = new
                                        {
                                            status = Helpers.CreateNativeFunction((args, env, expr) =>
                                            {
                                                resp.StatusCode = ((IntegerValue)args[0]).Value;
                                                return Helpers.CreateNull();
                                            }, options: new()
                                            {
                                                Name = "status",
                                                Parameters =
                                                {
                                                    new()
                                                    {
                                                        Name = "statusCode",
                                                        Type = Values.ValueType.Int
                                                    }
                                                }
                                            }),

                                            send = Helpers.CreateNativeFunction((args, env, expr) =>
                                            {
                                                if (respondedTo)
                                                {
                                                    throw new RuntimeException(new()
                                                    {
                                                        Location = expr.Location,
                                                        Error = $"Already sent to this request"
                                                    });
                                                }

                                                byte[] outt = Encoding.UTF8.GetBytes(((StringValue)args[0]).Value);

                                                try
                                                {
                                                    if (resp.OutputStream.CanWrite)
                                                    {
                                                        resp.OutputStream.Write(outt, 0, outt.Length);
                                                        resp.Close();
                                                    }
                                                } catch (Exception e)
                                                {
                                                    Debug.Error(e.ToString());
                                                    throw new RuntimeException(new()
                                                    {
                                                        Location = expr.Location,
                                                        Error = e.Message
                                                    });
                                                }

                                                respondedTo = true;
                                                return Helpers.CreateNull();
                                            }, options: new()
                                            {
                                                Name = "send",
                                                Parameters =
                                                {
                                                    new()
                                                    {
                                                        Name = "body",
                                                        Type = Values.ValueType.String
                                                    }
                                                }
                                            })
                                        }
                                    })
                                }, env);
                            }
                        }

                        listener.Start();

                        Task listenTask = HandleIncomingConnections();
                        listenTask.GetAwaiter().GetResult();
                    }));

                    return Helpers.CreateNull();
                }, options: new()
                {
                    Name = "start"
                })
            });
        }
    }
}
