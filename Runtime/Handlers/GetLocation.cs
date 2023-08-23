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
        public static Location? GetLocation(params Location?[] locations)
        {
            foreach (var location in locations)
            {
                if (location != null)
                    return location;
            }
            return null;
        }

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
