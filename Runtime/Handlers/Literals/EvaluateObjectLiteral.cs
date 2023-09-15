using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static Values.RuntimeValue EvaluateObjectLiteral(Parser.AST.ObjectLiteral o, Environment environment)
        {
            ObjectValue obj = new();
            obj.Location = o.Location;
            
            foreach (Property prop in o.Properties)
            {
                // Check if already added
                if (obj.Properties.ContainsKey(prop.Key))
                {
                    throw new RuntimeException(new()
                    {
                        Location = Helpers.GetLocation(prop.Location, obj.Location, o.Location),
                        Error = $"The object already contains the key {prop.Key}"
                    });
                }

                // Check if shorthand
                if (prop.IsAlone)
                {
                    RuntimeValue val = environment.LookupVariable(prop.Key);
                    obj.Properties.Add(prop.Key, val);
                } else
                {
                    obj.Properties.Add(prop.Key, Interpreter.Evaluate(prop.Value, environment));
                }
            }

            return obj;
        }
    }
}
