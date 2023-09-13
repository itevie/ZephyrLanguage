using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    /// <summary>
    /// Member expression is, for example, when you access an object, e.g., a.b
    /// </summary>
    internal class MemberExpression : Expression
    {
        /// <summary>
        /// The object where the property is, (a.b, a is the object)
        /// </summary>
        public AST.Expression Object = new();

        /// <summary>
        /// The property of the object to get (a.b, b is the property)
        /// </summary>
        public AST.Expression Property = new();

        /// <summary>
        /// Whether or not the property is computed, (a["b"])
        /// </summary>
        public bool IsComputed = false;

        public MemberExpression()
        {
            Kind = Kind.MemberExpression;
        }
    }
}
