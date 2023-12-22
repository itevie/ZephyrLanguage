using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ImportStatement : Expression
    {
        public StringLiteral From;
        public Identifier? ImportAs = null;
        public List<Identifier>? ImportIdentifiers = null;

        public ImportStatement(Location location, StringLiteral from, Identifier importAs)
            : base(Kind.ImportStatement, location)
        {
            From = from;
            ImportAs = importAs;
        }

        public ImportStatement(Location location, StringLiteral from, List<Identifier> identifierList)
            : base(Kind.ImportStatement, location)
        {
            From = from;
            ImportIdentifiers = identifierList;
        }
    }
}
