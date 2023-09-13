using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    /// <summary>
    /// Represents a function parameter
    /// </summary>
    internal class ParameterExpression : Expression
    {
        public TypeExpression Type = new TypeExpression();
        public Identifier Name = new Identifier();
        public Expression DefaultValue = new Expression();
    }
}
