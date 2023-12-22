using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class AssignmentExpression : Expression
    {
        public Expression Assignee;
        public string Operator;
        public Expression Value;

        public AssignmentExpression(Location location, Expression assignee, string @operator, Expression value)
            : base(Kind.AssignmentExpression, location)
        {
            Assignee = assignee;
            Operator = @operator;
            Value = value;
        }
    }
}
