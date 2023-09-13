using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;

namespace Zephyr.Runtime.Values
{
    /// <summary>
    /// Base value that all other types extend.
    /// </summary>
    internal class RuntimeValue
    {
        /// <summary>
        /// The type of the variable
        /// </summary>
        public ValueType Type { get; set; } = ValueType.Any;

        /// <summary>
        /// Whether or not this variable is marked as return, the name of this is ~return, it is set once the return keyword is used
        /// Once the function scope exits, ~return is read for the returned value
        /// </summary>
        public bool IsReturn = false;

        /// <summary>
        /// The location of the value - this is unreliable
        /// </summary>
        public Location? Location { get; set; } = null;

        /// <summary>
        /// The list of modifiers this value has
        /// </summary>
        public List<Modifier> Modifiers { get; set; } = new();

        /// <summary>
        /// Checks whether or not the value has the final modifier
        /// </summary>
        /// <returns></returns>
        public bool IsFinal()
        {
            return Modifiers.Contains(Modifier.Final);
        }
    }
}
