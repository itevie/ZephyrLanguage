using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class UnaryExpression : Expression
    {
        public Expression Value;
        public string Operator;

        public UnaryExpression(Location location, Expression value, string @operator)
            : base(Kind.UnaryExpression, location)
        {
            Value = value;
            Operator = @operator;
        }
    }
}
