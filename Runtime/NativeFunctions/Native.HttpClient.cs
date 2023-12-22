using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package HttpClientPackage = new Package("HTTPClient", new Values.ObjectValue(new
        {
            createClient = new NativeFunction((o) =>
            {
                HttpClient client = new HttpClient();
                ObjectValue headers = new ObjectValue();

                void updateHeaders()
                {
                    client.DefaultRequestHeaders.Clear();

                    foreach (KeyValuePair<string, RuntimeValue> header in headers.Properties)
                    {
                        // Check if it is string
                        if (header.Value.Type.TypeName != Values.ValueType.String)
                            throw new RuntimeException($"All headers must be of type string", header.Value.Location);

                        client.DefaultRequestHeaders.Add(header.Key, ((StringValue)header.Value).Value);
                    }
                };

                return new ObjectValue(new
                {
                    headers,

                    get = new NativeFunction((o) =>
                    {
                        updateHeaders();

                        Uri url = new Uri(((StringValue)o.Arguments[0]).Value);

                        HttpResponseMessage result;

                        try
                        {
                            result = client.GetAsync(url).GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            throw new RuntimeException($"HTTP error: {e.Message}", o.Location);
                        }

                        return new ObjectValue(new
                        {
                            statusCode = result.StatusCode,
                            success = result.IsSuccessStatusCode,
                            content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                        });
                    }, new Options("get", new List<Parameter>()
                    {
                        new Parameter(new VariableType(Values.ValueType.String), "url")
                    })),

                    delete = new NativeFunction((o) =>
                    {
                        updateHeaders();

                        Uri url = new Uri(((StringValue)o.Arguments[0]).Value);

                        HttpResponseMessage result;

                        try
                        {
                            result = client.DeleteAsync(url).GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            throw new RuntimeException($"HTTP error: {e.Message}", o.Location);
                        }

                        return new ObjectValue(new
                        {
                            statusCode = result.StatusCode,
                            success = result.IsSuccessStatusCode,
                            content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                        });
                    }, new Options("delete", new List<Parameter>()
                    {
                        new Parameter(new VariableType(Values.ValueType.String), "url")
                    })),

                    post = new NativeFunction((options) =>
                    {
                        updateHeaders();

                        // Collect data
                        Uri url = new Uri(((StringValue)options.Arguments[0]).Value);
                        string contents = ((StringValue)options.Arguments[1]).Value;
                        ObjectValue requestOptions = ((ObjectValue)options.Arguments[2]);
                        string mediaType = ((StringValue)requestOptions.Properties["mediaType"]).Value;

                        HttpResponseMessage result;

                        try
                        {
                            result = client
                                .PostAsync(url, new StringContent(contents, System.Text.Encoding.UTF8, mediaType))
                                .GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            throw new RuntimeException($"HTTP error: {e.Message}", options.Location);
                        }

                        return new ObjectValue(new
                        {
                            statusCode = result.StatusCode,
                            success = result.IsSuccessStatusCode,
                            content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                        });
                    }, new Options("post", new List<Parameter>()
                    {
                        new Parameter(new VariableType(Values.ValueType.String), "url"),
                        new Parameter(new VariableType(Values.ValueType.String), "body"),
                        new Parameter(new VariableType(Values.ValueType.Object)
                        {
                            IsStruct = true,
                            StructFields = new Dictionary<string, VariableType>()
                            {
                                { "mediaType", new VariableType(Values.ValueType.String) },
                            }
                        }, "options"),
                    }))
                });
            }, new Options("createClient")),
        }));
    }
}
