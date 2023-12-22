using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class LogicalExpression : Expression
    {
        public Expression Left;
        public string Operator;
        public Expression Right;

        public LogicalExpression(Location location, Expression left, string @operator, Expression right)
            : base(Kind.LogicalExpression, location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }
}
