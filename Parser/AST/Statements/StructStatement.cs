using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class StructStatement : Expression
    {
        public string Name { get; set; } = "";
        public List<Expression> Properties { get; set; } = new();
    }
}
