using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;
using Vals = Zephyr.Runtime.Values.Helpers.Helpers;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static RuntimeValue CastValueHelper(RuntimeValue value, Values.ValueType newType, Parser.AST.CastExpression? from = null)
        {
            switch (value.Type)
            {
                case Values.ValueType.Null:
                    switch (newType)
                    {
                        case Values.ValueType.String:
                            return Vals.CreateString("null");
                    }
                    break;
                case Values.ValueType.Number:
                case Values.ValueType.Int:
                case Values.ValueType.Long:
                    int numValue = (int)((IntegerValue)(Vals.CastNonFloatNumberValues(value, Values.ValueType.Int))).Value;

                    switch (newType)
                    {
                        case Values.ValueType.String:
                            return Vals.CreateString(numValue.ToString());
                        case Values.ValueType.Boolean:
                            return Vals.CreateBoolean(numValue > 0);
                        case Values.ValueType.Int:
                        case Values.ValueType.Long:
                        case Values.ValueType.Number:
                            return Vals.CastNonFloatNumberValues(value, newType);
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

                    switch (newType)
                    {
                        case Values.ValueType.Number:
                            float val = 0;
                            if (float.TryParse(stringValue, out val) == false)
                            {
                                throw new RuntimeException(new()
                                {
                                    Location = from?.Left.Location,
                                    Error = $"Cannot convert this to a number"
                                });
                            }

                            return Vals.CreateNumber(val);
                    }
                    break;
            }

            // By now, it has not succeeded
            throw new RuntimeException(new()
            {
                Location = from?.Location,
                Error = $"Cannot cast {value.Type} -> {newType}"
            });
        }
    }
}
