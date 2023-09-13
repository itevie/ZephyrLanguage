using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    /// <summary>
    /// Used for every name, in variable decl., func. names, etc.
    /// </summary>
    internal class Identifier : Expression
    {
        /// <summary>
        /// Symbol = name
        /// </summary>
        public string Symbol { get; set; } = "";

        public Identifier()
        {
            Kind = Kind.Identifier;
        }
    }
}
