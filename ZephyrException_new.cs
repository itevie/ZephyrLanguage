using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr
{
    internal class ZephyrException_new : Exception
    {
        public Expression? Expression { get; set; } = null;
        public Token? Token { get; set; } = null;
        public Location? Location { get; set; } = null;
        public RuntimeValue? Reference { get; set; } = null;
        public Expression? DeclaredAt { get; set; } = null;
        public Errors.ErrorType? ErrorType { get; set; } = null;
        public Errors.ErrorCode ErrorCode { get; set; } = Errors.ErrorCode.Generic;
        public Exception? MoreDetails { get; set; } = null;
        public string? Error = null;

        // For in Zephyr
        public string SafeMessage = "";

        public ZephyrException_new() : base()
        {

        }

        public string Visualise()
        {
            string finished = "";
            Location? location = Runtime.Handlers.Helpers.GetLocation(Location, Token?.Location, Expression?.Location, Expression?.FullExpressionLocation);

            // Check if there is locational information
            finished += GenerateLocation(location);

            finished += $"\n\n{ErrorType}[{(int)ErrorCode}]: ".Pastel(ConsoleColor.Red);

            string error;

            // Construct the error message
            if (Error != null)
            {
                error = Error;
            } else
            {
                error = Errors.ErrorGenerators.GenerateError(ErrorCode, this);
            }

            finished += error;
            SafeMessage = error;

            // Check for reference
            if (Reference != null)
            {
                finished += $"\n\n{VisualiseReference(Reference)}";
            }

            // Check for DeclaredAt
            if (DeclaredAt != null)
            {
                Location? decLoc = Runtime.Handlers.Helpers.GetLocation(DeclaredAt?.Location, DeclaredAt?.FullExpressionLocation);

                if (decLoc != null)
                {
                    finished += $"\n\nDeclared Here:\n".Pastel(ConsoleColor.Blue) + GenerateLocation(decLoc);
                }
            }

            // Check for a hint
            if (ErrorCode != Errors.ErrorCode.Generic)
            {
                if (Errors.ErrorHints.Hints.ContainsKey(ErrorCode))
                {
                    finished += $"\n\nHint:".Pastel(ConsoleColor.Blue) + $"\n{Errors.ErrorHints.Hints[ErrorCode]}".Pastel(ConsoleColor.Cyan);
                }
            }

            return finished;
        }

        public static string GenerateLocation(Location location)
        {
            string finished = "";

            if (location != null)
            {
                // Get the line from source
                int lineNumber = location.Line;
                string line = location.Source.Split('\n')[lineNumber].Replace("\t", " ");

                // Check if should modify TokenEnd
                if (location.TillEnd) location.TokenEnd = line.Length - 1;

                // Add where
                finished += $"{location.FileName}:[Line {location.Line + 1} Char {location.TokenStart + 1}{(location.TokenEnd == (location.TokenStart + 1) ? "" : ("-" + location.TokenEnd))}]";

                finished += "\n\n" + line.Pastel(ConsoleColor.Gray);

                // Add carets
                int caretLength = location.TokenEnd - location.TokenStart;
                if (caretLength == 0) caretLength = 1;

                try
                {
                    finished += $"\n{string.Concat(Enumerable.Repeat(" ", location.TokenStart))}{string.Concat(Enumerable.Repeat("^", caretLength))}{(location.TillEnd ? "..." : "")}".Pastel(ConsoleColor.Yellow); ;
                }
                catch { }
            }

            return finished;
        }

        public static string VisualiseReference(RuntimeValue val)
        {
            string done = "Reference: ".Pastel(ConsoleColor.Blue);

            switch (val.Type)
            {
                case Runtime.Values.ValueType.NativeFunction:
                    NativeFunction nfVal = (NativeFunction)val;
                    done += $"NativeFunction ".Pastel(ConsoleColor.Green) + $"{(nfVal.Name == "" ? nfVal.Options.Name : nfVal.Name).Pastel(ConsoleColor.Cyan)}(";

                    // Add params
                    for (int i = 0; i < nfVal.Options.Parameters.Count; i++)
                    {
                        NativeFunctionParameter param = nfVal.Options.Parameters[i];

                        done += $"{param.Type.ToString().ToLower().Pastel(ConsoleColor.Green)} {param.Name.Pastel(ConsoleColor.Cyan)}{(i != nfVal.Options.Parameters.Count - 1 ? ", " : "")}";
                    }

                    if (nfVal.Options.AllParamsOfType != null)
                    {
                        done += $"{nfVal.Options.AllParamsOfType?.ToString().ToLower().Pastel(ConsoleColor.Green)}...";
                    }
                    else if (nfVal.Options.UncheckedParameters)
                    {
                        done += "?...?";
                    }

                    done += ");";

                    if (nfVal.Options.Description != null)
                    {
                        done += $"\nDescription: {nfVal.Options.Description}".Pastel(ConsoleColor.Gray);
                    }
                    break;
            }

            return done;
        }
    }
}
