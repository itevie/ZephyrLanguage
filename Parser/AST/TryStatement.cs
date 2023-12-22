using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class TryStatement : Expression
    {
        public Expression Body;
        public Expression? Catch = null;
        public Identifier? CatchIdentifier = null;

        public TryStatement(Location location, Expression body, Expression? @catch = null, Identifier? catchIdent = null)
            : base(Kind.TryStatement, location)
        {
            NeedsSemicolon = false;
            Body = body;
            Catch = @catch;
            CatchIdentifier = catchIdent;
        }
    }
}
