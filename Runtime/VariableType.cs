using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime
{
    /// <summary>
    /// Contains every single possible information about a values types
    /// IsArray and IsStruct can be stacked for example: struct Dog[][][]
    /// </summary>
    internal class VariableType
    {
        // ----- Basic type information -----

        /// <summary>
        /// The overall type of the value, this will be Array despite the type of array
        /// </summary>
        public Values.ValueType TypeName { get; set; }

        /// <summary>
        /// Whether or not this value can be null
        /// </summary>
        public bool IsNullable = false;

        // ----- Stuff to do with arrays -----

        /// <summary>
        /// Whether or not the value is an array
        /// </summary>
        public bool IsArray = false;

        /// <summary>
        /// The type of the array string[] would mean this is string
        /// </summary>
        public Values.ValueType ArrayType { get; set; }

        /// <summary>
        /// How deep the array goes, for example string[] = 1 and string[][] = 2
        /// </summary>
        public int ArrayDepth = 0;

        // ----- Struct information -----
        
        /// <summary>
        /// Whether or not the value is a struct
        /// </summary>
        public bool IsStruct = false;

        /// <summary>
        /// The list of fields that are within the struct
        /// </summary>
        public Dictionary<string, VariableType> StructFields = new Dictionary<string, VariableType>();

        public List<VariableType>? GenericsList = null;

        /// <summary>
        /// Sets up the type with a basic type, no array or struct information
        /// </summary>
        /// <param name="valueType"></param>
        public VariableType(Values.ValueType valueType)
        {
            TypeName = valueType;
        }

        /// <summary>
        /// Sets up the type based on a type AST node
        /// </summary>
        /// <param name="type"></param>
        public VariableType(Parser.AST.Type type, Environment environment, Location location)
        {
            // Check if type can have generics
            if (type.GenericsList != null && !Helpers.CanHaveGenerics(type.TypeName))
                throw new RuntimeException($"The type {Helpers.VisualiseType(type)} cannot have generics", location);

            // Check if it is a struct
            if (type.IsStruct)
            {
                IsStruct = true;

                RuntimeValue structValue = Interpreter.Evaluate(type.StructIdentifier, environment);

                // Check type
                if (structValue.Type.TypeName != Values.ValueType.Struct)
                    throw new RuntimeException($"Cannot use a {Values.Helpers.VisualiseType(structValue.Type)} as a struct", location);

                StructFields = ((StructValue)structValue).Fields;
            }

            TypeName = type.IsArray ? Values.ValueType.Array : type.TypeName;
            ArrayType = type.TypeName;
            IsNullable = type.IsNullable;
            IsArray = type.IsArray;
            ArrayDepth = type.ArrayDepth;
            GenericsList = type.GenericsList?.Select(x => new VariableType(x, environment, location)).ToList();
        }

        public static VariableType AnyArray = new VariableType(Values.ValueType.Array)
        {
            ArrayType = Values.ValueType.Any,
            IsNullable = false,
            IsArray = true,
            ArrayDepth = 1
        };
    }
}
