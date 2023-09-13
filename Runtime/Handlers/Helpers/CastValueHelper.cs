using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Vals = Zephyr.Runtime.Values.Helpers.Helpers;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue CastValueHelper(RuntimeValue value, Values.ValueType newType, Location? loc = null)
        {
            // Check if same
            if (value.Type == newType)
            {
                return value;
            }

            // Check if any
            if (newType == Values.ValueType.Any)
            {
                return value;
            }

            switch (value.Type)
            {
                case Values.ValueType.Null:
                    switch (newType)
                    {
                        case Values.ValueType.String:
                            return Vals.CreateString("null");
                    }
                    break;
                case Values.ValueType.Float:
                case Values.ValueType.Int:
                case Values.ValueType.Long:
                    double numValue = 0;

                    if (value.Type == Values.ValueType.Float)
                        numValue = ((FloatValue)value).Value;
                    else if (value.Type == Values.ValueType.Int)
                        numValue = ((IntegerValue)value).Value;
                    else if (value.Type == Values.ValueType.Long)
                        numValue = ((LongValue)value).Value;

                    switch (newType)
                    {
                        case Values.ValueType.String:
                            return Vals.CreateString(numValue.ToString());
                        case Values.ValueType.Boolean:
                            return Vals.CreateBoolean(numValue > 0);
                        case Values.ValueType.Int:
                            return Vals.CreateInteger((int)numValue);
                        case Values.ValueType.Long:
                            return Vals.CreateLongValue((long)numValue);
                        case Values.ValueType.Float:
                            return Vals.CreateFloat((double)numValue);
                    }
                    break;
                case Values.ValueType.Boolean:
                    bool boolValue = ((BooleanValue)value).Value;

                    switch (newType)
                    {
                        case Values.ValueType.Number:
                            return Vals.CreateNumber(boolValue == true ? 1 : 0);
                        case Values.ValueType.String:
                            return Vals.CreateString(boolValue == true ? "true" : "false");
                    }
                    break;
                case Values.ValueType.String:
                    string stringValue = ((StringValue)value).Value;
                    break;
            }

            // By now, it has not succeeded
            throw new RuntimeException(new()
            {
                Location = loc,
                Error = $"Cannot cast {value.Type} -> {newType}"
            });
        }
    }
}
