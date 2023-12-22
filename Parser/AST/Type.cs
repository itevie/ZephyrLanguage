using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class Type
    {
        public Runtime.Values.ValueType TypeName;

        /// <summary>
        /// Whether or not this type is nullable (usually expressed with ? after type)
        /// </summary>
        public bool IsNullable;

        /// <summary>
        /// Whether or not the type is an array string[]
        /// </summary>
        public bool IsArray = false;

        /// <summary>
        /// The depth of the array if it is an array, e.g., string[][] = 2 depth
        /// </summary>
        public int ArrayDepth = 0;

        /// <summary>
        /// The location of the type
        /// </summary>
        public Location Location;

        public List<AST.Type>? GenericsList = null;

        public bool IsStruct = false;
        public Expression? StructIdentifier = null;

        public Type(Runtime.Values.ValueType typeName, bool isNullable, Location location)
        {
            TypeName = typeName;
            IsNullable = isNullable;
            Location = location;
        }
    }
}
