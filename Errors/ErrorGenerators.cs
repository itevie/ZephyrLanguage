using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Errors
{
    internal class ErrorGenerators
    {
        public static string GenerateError(ErrorCode code, ZephyrException_new e)
        {
            return code switch
            {
                ErrorCode.MissingSemiColon => $"Missing semi-colon at end of line{(e.Token != null ? $", got {e.Token.TokenType}" : "")}",
                ErrorCode.ModifierAlreadyUsed => $"This modifier was already used in this statement",
                ErrorCode.UnassignedConstantVariable => $"Cannot declare constant variable without initial assignment",
                ErrorCode.CannotReturnHere => "Cannot return here",
                ErrorCode.MissingdImportStatementIdentifier => $"Expected a string literal containing the path to a module",
                ErrorCode.AssignmentToConstantVariable => "Cannot assign to a constant variable",
                ErrorCode.VariableAlreadyExists => "The variable already exists",
                _ => $"Error code {(int)code} {code}"
            };
        }
    }
}
