using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class DecoratorApplierStatement : Expression
    {
        public Expression Applyee;
        public Expression Body;

        public DecoratorApplierStatement(Location location, Expression applyee, Expression body)
            : base(Kind.DecoratorApplierStatement, location)
        {
            NeedsSemicolon = false;
            Applyee = applyee;
            Body = body;
        }
    }
}
