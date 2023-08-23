using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static EnumerableValue CreateEnumerableValue(RuntimeValue toConvert)
        {
            EnumerableValue value = new EnumerableValue();

            switch (toConvert.Type)
            {
                case ValueType.String:
                    value = new EnumerableValue()
                    {
                        Values = new List<RuntimeValue>(((StringValue)toConvert).Value.ToCharArray().Select(x => CreateString(x.ToString())).ToList())
                    };
                    break;
                case ValueType.Array:
                    value = new EnumerableValue()
                    {
                        Values = ((ArrayValue)toConvert).Items
                    };
                    break;
                case ValueType.Enumerable:
                    return (EnumerableValue)toConvert;
                default:
                    throw new RuntimeException(new()
                    {
                        Location = toConvert.Location,
                        Error = $"Type {toConvert.Type} is not enumerable"
                    });
            }

            return value;
        }
    }
}
