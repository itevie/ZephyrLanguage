using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class ObjectValue : RuntimeValue
    {
        public ObjectValue()
        {
            Type = new VariableType(ValueType.Object);
        }

        public ObjectValue(object obj)
        {
            Type = new VariableType(ValueType.Object);

            // Check type
            if (obj is string or int or bool or float or double)
            {
                throw new Exception($"Cannot create an object from a {obj.GetType()}");
            }

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                try
                {
                    string name = property.Name;
                    object? value = property.GetValue(obj) ?? throw new Exception($"Got null whilst creating object (prop {name})");

                    // Check types - int
                    if (value is int intv)
                    {
                        Properties.Add(name, new NumberValue(intv));
                    }

                    else if (value is double doublev)
                    {
                        Properties.Add(name, new NumberValue(doublev));
                    }

                    // Check if it is already runtimevalue
                    else if (value is RuntimeValue rvv)
                    {
                        Properties.Add(name, rvv);
                    }

                    else if (value is string strv)
                    {
                        Properties.Add(name, new StringValue(strv));
                    }

                    else if (value is bool boolv)
                    {
                        Properties.Add(name, new BooleanValue(boolv));
                    }

                    // Object
                    else if (value is object objv)
                    {
                        Properties.Add(name, new ObjectValue(objv));
                    }

                    // Does not know how to handle this
                    else
                    {
                        throw new Exception($"Cannot handle type {value.GetType()} for property {name}");
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            return Helpers.VisualiseObject(Properties);
        }
    }
}
