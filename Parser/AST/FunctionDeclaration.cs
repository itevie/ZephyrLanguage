using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class FunctionDeclaration : Expression
    {
        public Expression? Name { get; set; } = new Expression();
        public List<Expression> Parameters { get; set; } = new List<Expression>();
        public Expression Body { get; set; } = new Expression();

        public FunctionDeclaration()
        {
            Kind = Kind.FunctionDeclaration;
        }
    }
}
