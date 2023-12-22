using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime;

namespace ZephyrNew.Parser.AST
{
    internal class ApplyModifierStatement : Expression
    {
        public Expression Value;
        public Modifier Modifier;

        public ApplyModifierStatement(Location location, Expression value, Modifier modifier)
            : base(Kind.ApplyModifierStatement, location)
        {
            NeedsSemicolon = false;
            Value = value;
            Modifier = modifier;
        }
    }
}
