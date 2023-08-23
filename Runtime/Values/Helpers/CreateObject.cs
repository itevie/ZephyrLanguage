using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static ObjectValue CreateObject(object obj)
        {
            // Check type
            if (obj is string or int or bool or float or double)
            {
                Debug.Error(new StackFrame(100, true).ToString());
                throw new Exception($"Cannot create an object from a {obj.GetType()}");
            }

            Dictionary<string, RuntimeValue> properties = new();

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                try
                {
                    string name = property.Name;
                    object? value = property.GetValue(obj) ?? throw new Exception($"Got null whilst creating object (prop {name})");

                    // Check types - int
                    if (value is int intv)
                    {
                        properties.Add(name, CreateInteger(intv));
                    }

                    // Check if it is already runtimevalue
                    else if (value is RuntimeValue rvv)
                    {
                        properties.Add(name, rvv);
                    }

                    // Native function
                    else if (value is Func<List<RuntimeValue>, Environment, Parser.AST.Expression, RuntimeValue> nvv)
                    {
                        properties.Add(name, CreateNativeFunction(nvv));
                    }

                    else if (value is string strv)
                    {
                        properties.Add(name, CreateString(strv));
                    }

                    // Object
                    else if (value is object objv)
                    {
                        properties.Add(name, CreateObject(objv));
                    }

                    // Does not know how to handle this
                    else
                    {
                        throw new Exception($"Cannot handle type {value.GetType()} for property {name}");
                    }
                } catch (Exception e)
                {
                    Debug.Error($"Exception thrown: {e}\nWhilst trying to parse property\nJSON of obj: {JsonConvert.SerializeObject(obj)}", "create-object error");
                    Debug.Error($"Literal obj: {obj}\nType of obj: {obj.GetType()}", "create-object error");
                    Debug.Error(new StackFrame(100, true).ToString(), "create-object error");
                }
            }

            return new ObjectValue()
            {
                Properties = properties
            };
        } 
    }
}
