using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;

namespace Zephyr.Parser.AST
{
    internal class AssignmentExpression : Expression
    {
        public Expression Assignee { get; set; } = new Expression();
        public Expression Value { get; set; } = new Expression();
        public string Type { get; set; } = "=";

        public AssignmentExpression()
        {
            Kind = Kind.AssignmentExpression;
        }
    }
}
