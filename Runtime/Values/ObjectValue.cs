using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;

namespace Zephyr.Runtime.Values
{
    internal class ObjectValue : RuntimeValue
    {
        public Dictionary<string, RuntimeValue> Properties = new();

        public void AddProperty(string key, RuntimeValue value, Location loc)
        {
            CanModify(loc);

            if (this.Type == ValueType.Struct)
            {
                // Check if the object complies with the struct
            }
        }

        private void CanModify(Location loc)
        {
            if (IsFinal())
            {
                throw new RuntimeException(new()
                {
                    Location = Handlers.Helpers.GetLocation(loc),
                    Error = $"The object is marked as final and cannot be edited"
                });
            }
        }

        public ObjectValue()
        {
            Type = ValueType.Object;
        }
    }
}
