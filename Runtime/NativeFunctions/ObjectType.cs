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
        public static Package ObjectType = new("Object", new
        {
            addProperty = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                // Check if allowed to assign to it
                if (((ObjectValue)args[0]).IsFinal())
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location, args[1].Location),
                        Error = $"The object is marked as final and cannot be edited"
                    });
                }

                if (((ObjectValue)args[0]).Properties.ContainsKey(((StringValue)args[1]).Value))
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(args[0].Location, args[1].Location, expr?.Location),
                        Error = $"The object already contains this property"
                    });
                }

                ((ObjectValue)args[0]).Properties.Add(((StringValue)args[1]).Value, args[2]);
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "addProperty",
                Parameters =
                {
                    new()
                    {
                        Name = "obj",
                        Type = Values.ValueType.Object
                    },
                    new()
                    {
                        Name = "key",
                        Type = Values.ValueType.String
                    },
                    new()
                    {
                        Name = "value",
                        Type = Values.ValueType.Any
                    }
                }
            }),

            hasProperty = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                if (((ObjectValue)args[0]).Properties.ContainsKey(((StringValue)args[1]).Value))
                {
                    return Helpers.CreateBoolean(true);
                }

                return Helpers.CreateBoolean(false);
            }, options: new()
            {
                Name = "hasProperty",
                Parameters =
                {
                    new()
                    {
                        Name = "obj",
                        Type = Values.ValueType.Object
                    },
                    new()
                    {
                        Name = "property",
                        Type = Values.ValueType.String
                    }
                }
            }),

            removeProperty = Helpers.CreateNativeFunction((args, env, expr) =>
            {
                // Check if allowed to assign to it
                if (((ObjectValue)args[0]).IsFinal())
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location, args[1].Location),
                        Error = $"The object is marked as final and cannot be edited"
                    });
                }

                // Check if object contains the property
                if (((ObjectValue)args[0]).Properties.ContainsKey(((StringValue)args[1]).Value) == false)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(args[0].Location, args[1].Location, expr?.Location),
                        Error = $"The object does not contain this property"
                    });
                }

                ((ObjectValue)args[0]).Properties.Remove(((StringValue)args[1]).Value);
                return Helpers.CreateNull();
            }, options: new()
            {
                Name = "removeProperty",
                Parameters =
                {
                    new()
                    {
                        Name = "obj",
                        Type = Values.ValueType.Object
                    },
                    new()
                    {
                        Name = "key",
                        Type = Values.ValueType.String
                    },
                }
            }),
        });
    }
}
