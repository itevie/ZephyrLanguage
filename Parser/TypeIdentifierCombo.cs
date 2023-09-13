using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser.AST;

namespace Zephyr.Parser
{
    /// <summary>
    /// This contains a type and an identifier, this is used everywhere
    /// Var. decl., param.: int? a
    /// </summary>
    internal class TypeIdentifierCombo
    {
        public Identifier Identifier;
        public Runtime.Values.ValueType Type = Runtime.Values.ValueType.Any;
        public bool isNullable = false;

        public TypeIdentifierCombo(Identifier identifer, Runtime.Values.ValueType type, bool isNullable)
        {
            Identifier = identifer;
            Type = type;
            this.isNullable = isNullable;
        }
    }
}
