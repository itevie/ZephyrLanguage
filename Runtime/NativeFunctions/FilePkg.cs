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
        public static Package FilePkg = new("Files", new
        {
            readAllText = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                try
                {
                    string text = File.ReadAllText(((StringValue)args[0]).Value);

                    return Helpers.CreateString(text);
                } catch (Exception e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0]?.Location),
                        Error = e.Message
                    });
                }
            }, options: new()
            {
                Name = "readAllText",
                PermissionsNeeded = "r",
                Parameters =
                {
                    new()
                    {
                        Name = "fileName",
                        Type = Values.ValueType.String
                    }
                }
            }),

            writeAllText = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                try
                {
                    File.WriteAllText(((StringValue)args[0]).Value, ((StringValue)args[1]).Value);
                    return Helpers.CreateNull();
                }
                catch (Exception e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0]?.Location),
                        Error = e.Message
                    });
                }
            }, options: new()
            {
                Name = "writeAllText",
                PermissionsNeeded = "w",
                Parameters =
                {
                    new()
                    {
                        Name = "fileName",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "toWrite",
                        Type = Values.ValueType.String
                    }
                }
            })
        });
    }
}
