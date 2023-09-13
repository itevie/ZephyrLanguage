using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        /// <summary>
        /// Loops through every given location and returns the first one that contains a non-null location
        /// </summary>
        /// <param name="locations">List of all locations to check</param>
        /// <returns>The first non-null location, or null if non was found</returns>
        public static Location? GetLocation(params Location?[] locations)
        {
            foreach (var location in locations)
            {
                if (location != null)
                    return location;
            }
            return null;
        }

        /// <summary>
        /// Loops through every given expression and returns the first one that contains a non-null location within it
        /// </summary>
        /// <param name="expressions">List of all expressions to check</param>
        /// <returns>The first non-null location, or null if non was found</returns>
        public static Expression? GetLocation(params Expression?[] expressions)
        {
            foreach (var expr in expressions)
            {
                if (expr?.Location != null)
                    return expr;
            }
            return null;
        }
    }
}
