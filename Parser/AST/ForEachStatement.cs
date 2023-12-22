using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ForEachStatement : Expression
    {
        public TypeNameCombo Declaration;
        public TypeNameCombo? DeclarationIndex = null;
        public Expression Right;
        public Expression Body;

        public ForEachStatement(Location location, TypeNameCombo declaration, Expression right, Expression body)
            : base(Kind.ForEachStatement, location)
        {
            NeedsSemicolon = false;
            Declaration = declaration;
            Right = right;
            Body = body;
        }
    }
}
