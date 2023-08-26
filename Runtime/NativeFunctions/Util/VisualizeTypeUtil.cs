using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Util
    {
        public static string VisualizeType(RuntimeValue value, bool showQuotes = false, int objectIndent = 0, bool noColors = false)
        {
            if (value == null)
                return "null".Pastel(ConsoleColor.Gray);

            // Check type
            switch (value.Type)
            {
                case Values.ValueType.Int:
                    if (noColors) return ((IntegerValue)value).Value.ToString();
                    return ((IntegerValue)value).Value.ToString().Pastel(ConsoleColor.Magenta);
                case Values.ValueType.Float:
                    string returning = ((FloatValue)value).Value.ToString();

                    // Check if should add .0
                    if (((FloatValue)value).Value % 1 == 0)
                    {
                        returning += ".0";
                    }

                    if (noColors) return returning.ToString();
                    return returning.Pastel(ConsoleColor.Magenta);
                case Values.ValueType.Double:
                    if (noColors) return ((DoubleValue)value).Value.ToString();
                    return ((DoubleValue)value).Value.ToString().Pastel(ConsoleColor.Magenta);
                case Values.ValueType.Long:
                    if (noColors) return ((LongValue)value).Value.ToString();
                    return ((LongValue)value).Value.ToString().Pastel(ConsoleColor.Magenta);
                case Values.ValueType.Null:
                    if (noColors) return "null";
                    return "null".Pastel(ConsoleColor.Gray);
                case Values.ValueType.Boolean:
                    BooleanValue bval = (BooleanValue)value;
                    if (bval.Value == true)
                        return noColors ? "true" : "true".Pastel(ConsoleColor.Green);
                    else return noColors ? "false" : "false".Pastel(ConsoleColor.Red);
                case Values.ValueType.String:
                    string str = $"{(showQuotes == true ? '"' : "")}{((StringValue)value).Value}{(showQuotes == true ? '"' : "")}";
                    if (showQuotes) str = str
                            .Replace("\n", "\\n")
                            .Pastel(ConsoleColor.Green);
                    return str;
                case Values.ValueType.NativeFunction:
                    if (noColors) return $"<NativeFunction {((NativeFunction)value).Name}>";
                    return $"<NativeFunction {((NativeFunction)value).Name}>".Pastel(ConsoleColor.Gray);
                case Values.ValueType.Function:
                    if (noColors) return $"<Function {((FunctionValue)value).Name}>";
                    return $"<Function {((FunctionValue)value).Name}>".Pastel(ConsoleColor.Gray);
                case Values.ValueType.Object:
                    string obj = "{";
                    ObjectValue objValue = (ObjectValue)value;
                    objectIndent += 2;

                    for (int i = 0; i < objValue.Properties.Count; i++)
                    {
                        obj += "\n" + string.Concat(Enumerable.Repeat(" ", objectIndent)) 
                            + objValue.Properties.ElementAt(i).Key 
                            + ": " 
                            + VisualizeType(objValue.Properties.ElementAt(i).Value, true, objectIndent, noColors);

                        if (i != objValue.Properties.Count - 1)
                            obj += ", ";
                    }

                    obj += $"\n{string.Concat(Enumerable.Repeat(" ", objectIndent - 2))}}}";

                    return obj;
                case Values.ValueType.Array:
                    ArrayValue arrValue = (ArrayValue)value;

                    string values = "";

                    for (int i = 0; i < arrValue.Items.Count; i++)
                    {
                        values += VisualizeType(arrValue.Items[i], true, noColors: noColors);
                        if (i != arrValue.Items.Count - 1)
                            values += ", ";
                    }

                    if (noColors) return $"<Array: {arrValue.ItemsType}, {arrValue.Items.Count} item{(arrValue.Items.Count != 1 ? "s" : "")} [{values}]>";
                    return $"<Array: {arrValue.ItemsType}, {arrValue.Items.Count} item{(arrValue.Items.Count != 1 ? "s" : "")} [{values}]>".Pastel(ConsoleColor.Gray);
                case Values.ValueType.Enumerable:
                    EnumerableValue enumerableValue = (EnumerableValue)value;

                    string enumValues = "";

                    for (int i = 0; i < enumerableValue.Values.Count; i++)
                    {
                        enumValues += VisualizeType(enumerableValue.Values[i], true);
                        if (i != enumerableValue.Values.Count - 1)
                            enumValues += ", ";
                    }

                    if (noColors)
                        return $"<Enumerable {enumValues}>".Pastel(ConsoleColor.Gray);
                    return $"<Enumerable {enumValues}>".Pastel(ConsoleColor.Gray);
                default:
                    if (noColors) return $"<Unknown {value.Type}>";
                    return $"<Unknown {value.Type}>".Pastel(ConsoleColor.Gray);
            }
        }
    }
}
