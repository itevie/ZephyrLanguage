using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class BinaryExpression : Expression
    {
        public Expression Left;
        public Expression Right;
        public string Operator;

        public BinaryExpression(Location location, Expression left, Expression right, string @operator) : base(Kind.BinaryExpression, location)
        {
            Right = right;
            Left = left;
            Operator = @operator;
        }
    }
}
