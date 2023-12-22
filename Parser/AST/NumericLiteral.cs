using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class NumericLiteral : Expression
    {
        public double Value;
        public bool IsReal { get; set; } = false;

        public NumericLiteral(Location location, double value) : base(Kind.NumericLiteral, location)
        {
            Value = value;
        }
    }
}
