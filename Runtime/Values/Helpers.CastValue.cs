using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Lexer.Syntax;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static RuntimeValue CastValue(RuntimeValue value, VariableType castTo, Location? location = null)
        {
            string operation = $"{Helpers.VisualiseType(value.Type)} {Operators.BasicOperators["Cast"].Symbol} {VisualiseType(castTo)}";

            // For non-array values
            if (castTo.IsArray == false && !value.Type.IsArray)
            {
                // Check if same type
                if (castTo.TypeName == value.Type.TypeName)
                    return value;

                switch (castTo.TypeName)
                {
                    // \? -> string
                    case ValueType.String:
                        // number -> string
                        if (value.Type.TypeName == Values.ValueType.Number)
                        {
                            return new StringValue("" + ((NumberValue)value).Value);
                        }

                        switch (value.Type.TypeName)
                        {
                            // bool -> string
                            case ValueType.Boolean:
                                return new StringValue(((BooleanValue)value).Value.ToString().ToLower());

                            // null -> string
                            case ValueType.Null:
                                return new StringValue("null");
                        }
                        break;
                }
            } else if (castTo.IsArray && value.Type.IsArray)
            {
                if (TypeMatches(castTo, value) == null)
                {
                    return value;
                } 
            }

            throw new RuntimeException($"Cannot perfrom {operation}", location != null ? location : Location.UnknownLocation);
        }
    }
}
