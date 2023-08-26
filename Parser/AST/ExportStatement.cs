using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class ExportStatement : Expression
    {
        public Expression ToExport = new();
        public Expression ExportAs = new();

        public ExportStatement()
        {
            Kind = Kind.ExportStatement;
        }
    }
}
