using System.Net.WebSockets;
using System.Text;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package WsPackage = new Package("WS", new ObjectValue(new
        {
            createClient = new NativeFunction((options) =>
            {
                ClientWebSocket ws = new ClientWebSocket();

                return new ObjectValue(new
                {
                    connect = new AsyncNativeFunction(async (op) =>
                    {
                        string url = ((StringValue)op.Arguments[0]).Value;
                        Uri uri = new Uri(url);

                        await ws.ConnectAsync(uri, CancellationToken.None);

                        return new NullValue();
                    }, new Options("start", new List<Parameter>()
                    {
                        new Parameter(new VariableType(Values.ValueType.String), "url"),
                    })),

                    isOpen = new NativeFunction((op) =>
                    {
                        return new BooleanValue(ws.State == WebSocketState.Open);
                    }, new Options("isOpen")),

                    send = new AsyncNativeFunction(async (op) =>
                    {
                        // Check if open
                        if (ws.State != WebSocketState.Open)
                            throw new RuntimeException($"WS error: Websocket is not open and is not ready to send messages", op.Location);

                        // Collect contents
                        string contents = ((StringValue)op.Arguments[0]).Value;
                        byte[] encoded = Encoding.UTF8.GetBytes(contents);
                        ArraySegment<byte> buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);

                        // Send
                        await ws.SendAsync(buffer.Array, WebSocketMessageType.Text, true, CancellationToken.None);

                        return new NullValue();
                    }, new Options("send", new List<Parameter>()
                    {
                        new Parameter(new VariableType(Values.ValueType.String), "str"),
                    })),

                    recieve = new AsyncNativeFunction(async (op) =>
                    {
                        // Check if open
                        if (ws.State != WebSocketState.Open)
                            throw new RuntimeException($"WS error: Websocket is not open and is not ready to recieve messages", op.Location);

                        var buffer = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = null;
                        var allBytes = new List<byte>();
                        try
                        {
                            do
                            {
                                result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                                for (int i = 0; i < result.Count; i++)
                                {
                                    allBytes.Add(buffer.Array[i]);
                                }
                            }
                            while (!result.EndOfMessage);
                        }
                        catch (Exception e)
                        {
                            throw new RuntimeException($"WS error: {e.Message}", op.Location);
                        }

                        string text = Encoding.UTF8.GetString(allBytes.ToArray(), 0, allBytes.Count);

                        return new StringValue(text);
                    }, new Options("recieve")),
                });
            }, new Options("createClient")),
        }));
    }
}
