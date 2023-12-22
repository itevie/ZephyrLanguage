using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using ZephyrNew.Errors;
using ZephyrNew.Runtime;

namespace ZephyrNew.Lexer
{
    internal class ZephyrException : Exception
    {
        /// <summary>
        /// The basic message for the error
        /// </summary>
        public string Error { get; set; } = "Unknown Error";

        /// <summary>
        /// The location in which this error occurred
        /// </summary>
        public Location Location;

        /// <summary>
        /// If an expression is given it will try to find the location from that if Location is not provided
        /// </summary>
        public Expression? Expression = null;

        /// <summary>
        /// In which part of the interpreter the error occurred
        /// </summary>
        public ErrorType ErrorType { get; set; } = ErrorType.Generic;

        public ZephyrException(string message, Location location) : base(message)
        {
            Error = message;
            Location = location;
        }

        public string Visualise()
        {
            string finished = "";
            finished += GenerateLocation(this.Location);
            finished += $"\n\n{ErrorType} error: ".Pastel(ConsoleColor.Red) + $"{Error}";

            //finished += $"\n\nC# StackTrace:\n{StackTrace}".Pastel(ConsoleColor.Gray);
            Stack stack = StackContainer.GetStack();
            if (stack.Stacktrace.Count > 0)
            {
                finished += $"\n\nStacktrace:\n{stack.Visualise()}".Pastel(ConsoleColor.Gray);
            }

            return finished;
        }

        public static string GenerateLocation(Location location)
        {
            string finished = "";

            int lineNumber = location.Line;

            // Get the source of the location
            string? source = location.Source != null ? Lexer.AllSources[location.Source] : null;
            string line = "<No location source provided>";

            if (source != null)
                line = source.Split('\n')[lineNumber].Replace("\t", "");

            // Compute where it happened
            int fromChar;
            int toChar = -1;

            if (location.TokenStart == location.TokenEnd || (location.TokenEnd == location.TokenStart + 1))
                fromChar = location.TokenStart + 1;
            else
            {
                fromChar = location.TokenStart + 1;
                toChar = location.TokenEnd;
            }

            // Add where it happened
            finished += $"{location.FileName}:[Line {lineNumber + 1} Char {fromChar}{(toChar != -1 ? "-" + toChar : "")}]{(location.AssumedLocation ? " (Assumed)" : "")}";
            finished += $"\n\n{line}".Pastel(ConsoleColor.Gray);

            // Add carets
            int caretLength = location.TokenEnd - location.TokenStart;
            if (caretLength == 0) caretLength = 1;

            try
            {
                // Construct the arrow
                string arrow = "";

                // Add the beginning padding
                arrow += string.Concat(Enumerable.Repeat(" ", location.TokenStart));

                // Check if the arrow should be ^---^ style
                if (caretLength > 2)
                {
                    arrow += "^";
                    arrow += string.Concat(Enumerable.Repeat("-", caretLength - 2));
                    arrow += "^";
                }
                else
                {
                    arrow += string.Concat(Enumerable.Repeat("^", caretLength));
                }

                finished += $"\n{arrow}".Pastel(ConsoleColor.Yellow);
            }
            catch { };

            return finished;
        }
    }
}
