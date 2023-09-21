using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Errors
{
    internal enum ErrorCode
    {
        Generic,

        // Syntax errors
        MissingSemiColon,
        ModifierAlreadyUsed,
        UnassignedConstantVariable,
        GenericUnexpectedToken,
        MissingdImportStatementIdentifier,
        CannotReturnHere,
        AssignmentToConstantVariable,
        VariableAlreadyExists,
        NullAssignment,
    }
}
