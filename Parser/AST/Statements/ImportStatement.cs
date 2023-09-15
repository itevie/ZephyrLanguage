using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ImportStatement : Expression
    {
        public Expression ToImport { get; set; } = new Expression();
        public Expression? ImportAs { get; set; } = new Expression();

        public ImportStatement()
        {
            Kind = Kind.ImportStatement;
        }
    }
}
