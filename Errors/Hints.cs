using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Errors
{
    internal class ErrorHints
    {
        public static Dictionary<ErrorCode, string> Hints = new Dictionary<ErrorCode, string>()
        {
            { ErrorCode.MissingSemiColon, "In Zephyr, all lines that aren't control-flow statements (e.g., ifs, function declarations) must end with a semi-conlon (;)" },
            { ErrorCode.UnassignedConstantVariable, "When using constant variables, you must initialise them with a value" },
            { ErrorCode.CannotReturnHere, "You cannot return in top-level statements, you can only return in function scopes" },
            { ErrorCode.AssignmentToConstantVariable, "Constant means the value can never be changed no matter what" },
            { ErrorCode.NullAssignment, "To allow null to be assigned to the variable, suffix the type with a question mark, e.g., var int" + "?".Pastel(ConsoleColor.Green) + " myVar;" }
        };
    }
}
