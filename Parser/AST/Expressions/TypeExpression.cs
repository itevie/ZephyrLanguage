using Zephyr.Runtime.Values;

namespace Zephyr.Parser.AST
{
    /// <summary>
    /// Represents a type used anywhere - variable decl., parameter, etc.
    /// </summary>
    internal class TypeExpression
    {
        public Runtime.Values.ValueType Type { get; set; } = Runtime.Values.ValueType.Any;
        public bool IsNullable { get; set; } = false;
        public bool IsArray { get; set; } = false;
    }
}
