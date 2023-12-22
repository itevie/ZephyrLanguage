using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="testWith"></param>
        /// <returns>Returns null if the type is matches, return string if it does not match, containing the error</returns>
        public static string? TypeMatches(VariableSettings settings, RuntimeValue testWith)
        {
            return TypeMatches(settings.Type, testWith);
        }

        public static string? GenericsMatch(List<VariableType> type, List<VariableType> testWith)
        {
            // Check lengths
            if (testWith.Count != type.Count)
                return $"Expected {type.Count} items in generic list but got {testWith.Count}";

            for (int i = 0; i != type.Count; i++)
            {
                if (!CompareTypes(type[i], testWith[i]))
                    return $"{VisualiseType(testWith[i])} does not match with {type[i]}";
            }

            return null;
        }

        public static string? TypeMatches(VariableType type, RuntimeValue testWith)
        {
            void throwError(string error, Location location)
            {
                throw new RuntimeException(error, location);
            }

            string typeStr = VisualiseType(type);
            string testWithStr = VisualiseType(testWith.Type);

            if (type.TypeName == ValueType.Any)
                return null;

            /*if (type.GenericsList != null)
            {
                // Check if the testing value has the generics
                if (testWith.Type.GenericsList == null)
                    throwError($"Expected value to have generics to match {typeStr} (got {testWithStr})", testWith.Location);

                string? error = GenericsMatch(type.GenericsList, testWith.Type.GenericsList);
                if (error != null)
                    throwError($"Generics do not match: {error}", testWith.Location);
            }*/

            // Check if it is an array
            if (type.IsArray)
            {
                // Check if the given value is an array
                if (testWith.Type.TypeName != Values.ValueType.Array)
                {
                    throwError($"{testWithStr} is not of type {typeStr}", testWith.Location);
                }

                ArrayValue array = (ArrayValue)testWith;
                long index = 0;

                foreach (RuntimeValue value in array.Items)
                {
                    // If the array depth is 0 then it is not an array and therefore the actual array type should
                    // be used instead of creating a new type of array.
                    VariableType t = type.ArrayDepth - 1 < 1 ? new VariableType(type.ArrayType)
                    {
                        IsNullable = type.IsNullable,
                        IsStruct = type.IsStruct,
                        StructFields = type.StructFields,
                    } : new VariableType(Values.ValueType.Array)
                    {
                        IsArray = type.ArrayDepth - 1 > 0,
                        ArrayDepth = type.ArrayDepth - 1,
                        IsNullable = type.IsNullable,
                        ArrayType = type.ArrayType,
                        IsStruct = type.IsStruct,
                        StructFields = type.StructFields,
                    };

                    try
                    {
                        TypeMatches(t, value);
                        if (type.ArrayDepth - 1 < 1)
                        {
                            t.TypeName = value.Type.TypeName;
                        }
                    } catch (RuntimeException e)
                    {
                        throwError($"[{index}] {e.Message}", e.Location);
                    }

                    // Update type
                    value.Type = t;
                    index++;
                }

                array.Type = type;

                return null;
            }

            // Check if it is a struct
            if (type.IsStruct)
            {
                // The type of the value should be an object
                if (testWith.Type.TypeName != Values.ValueType.Object)
                    throwError($"Expected an object value to test the struct on, but got {VisualiseType(testWith.Type)}", testWith.Location);

                ObjectValue value = (ObjectValue)testWith;

                // Compare fields
                foreach (KeyValuePair<string, VariableType> kv in type.StructFields)
                {
                    // Check if testWith has the property
                    if (kv.Value.IsNullable == false && value.Properties.ContainsKey(kv.Key) == false)
                        throwError($"Value does not match with struct: missing key: {kv.Key}", value.Location);
                    else if (kv.Value.IsNullable == true)
                    {
                        // Skip if not there
                        if (!value.Properties.ContainsKey(kv.Key))
                            continue;
                    }

                    // Check correct type
                    string? success = TypeMatches(kv.Value, value.Properties[kv.Key]);
                }
            }

            // Check for nullability
            if (testWith.Type.TypeName == ValueType.Null)
            {
                // Check if it is allowed to be null
                if (type.IsNullable == false) throwError($"the variable is not nullable, but the given value was null", testWith.Location);
                return null;
            }

            // Check if they are just the same
            if (type.TypeName != testWith.Type.TypeName) throwError($"{Helpers.VisualiseType(testWith.Type)} does not match with type {VisualiseType(type)}", testWith.Location);

            return null;
        }
    }
}
