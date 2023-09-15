using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;

namespace Zephyr.Parser.AST
{
    internal class VariableDeclaration : Expression
    {
        public bool IsConstant = false;
        public Identifier Identifier = new();
        public Expression? Value = null;

        // Type stuff
        public Runtime.Values.ValueType Type = Runtime.Values.ValueType.Any;
        public bool IsTypeNullable = false;

        // Modifiers
        public List<Runtime.Values.Modifier> Modifiers = new();

        public VariableDeclaration()
        {
            Kind = Kind.VariableDeclaration;
        }
    }
}
