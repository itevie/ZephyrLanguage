using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class VariableDeclaration : Expression
    {
        public Identifier Identifier;
        public Type Type;
        public Expression Value;

        public VariableDeclaration(Location location, Identifier identifier, Type type, Expression value) : base(Kind.VariableDeclaration, location)
        {
            Identifier = identifier;
            Type = type;
            Value = value;
        }
    }
}
