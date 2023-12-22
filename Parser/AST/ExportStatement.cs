using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ExportStatement : Expression
    {
        public Expression ToExport;

        public ExportStatement(Location location, Expression expr)
            : base(Kind.ExportStatement, location)
        {
            NeedsSemicolon = false;
            ToExport = expr;
        }
    }
}
