using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class UnaryRightExpression : Expression
    {
        public Expression Value;
        public string Operator;

        public UnaryRightExpression(Location location, Expression value, string @operator)
            : base(Kind.UnaryRightExpression, location)
        {
            Value = value;
            Operator = @operator;
        }
    }
}
