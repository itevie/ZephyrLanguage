using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Runtime.Values;

namespace Zephyr
{
    internal class ZephyrExceptionOptions
    {
        public Token? Token { get; set; } = null;
        public Location? Location { get; set; } = null;
        public string Error = "Unknown error";
        public RuntimeValue? Reference = null;
    }

    internal class ZephyrException : Exception
    {
        public Expression? Expression { get; set; } = null;
        public Token? Token { get; set; } = null;
        public Location? Location { get; set; } = null;
        public RuntimeValue? Reference { get; set; } = null;
        public Errors.ErrorType? ErrorType { get; set; } = null;
        public Errors.ErrorCode ErrorCode { get; set; } = Errors.ErrorCode.Generic;
        public Exception? MoreDetails { get; set; } = null;
        public string Error = "";

        public ZephyrException(ZephyrExceptionOptions options) : base(GenerateExceptionText(options))
        {

        }

        public ZephyrException(string msg, bool isOnlyString) : base(msg)
        {

        }

        public static string GenerateExceptionText(ZephyrExceptionOptions options)
        {
            string finished = "";

            // Check if there is a token
            if (options.Token != null || options.Location != null)
            {
                Location location = options.Token != null ? options.Token.Location : options.Location ?? throw new Exception();

                // Get the line from source
                int lineNumber = location.Line;
                string line = location.Source.Split('\n')[lineNumber].Replace("\t", " ");

                // Check if should modify TokenEnd
                if (location.TillEnd) location.TokenEnd = line.Length - 1;

                // Add where
                finished += $"{location.FileName}:[Line {location.Line + 1} Char {location.TokenStart + 1}{(location.TokenEnd == (location.TokenStart + 1) ? "" : ("-" + location.TokenEnd))}]";

                finished += "\n\n" + line;

                // Add carets
                int caretLength = location.TokenEnd - location.TokenStart;
                if (caretLength == 0) caretLength = 1;

                finished += $"\n{string.Concat(Enumerable.Repeat(" ", location.TokenStart))}{string.Concat(Enumerable.Repeat("^", caretLength))}{(location.TillEnd ? "..." : "")}";
            } else
            {
                finished += $"There was no location information provided with this error, sorry";
            }

            finished += $"\n\n";

            if (options.Reference != null)
            {
                finished += VisualiseReference(options.Reference) + "\n";
            }
            
            finished += options.Error;

            return finished;
        }

        public static string VisualiseReference(RuntimeValue val)
        {
            string done = "Ref: ";

            switch (val.Type)
            {
                case Runtime.Values.ValueType.NativeFunction:
                    NativeFunction nfVal = (NativeFunction)val;
                    done += $"NativeFunction {(nfVal.Name == "" ? nfVal.Options.Name : nfVal.Name)}(";

                    // Add params
                    for (int i = 0; i < nfVal.Options.Parameters.Count; i++)
                    {
                        NativeFunctionParameter param = nfVal.Options.Parameters[i];

                        done += $"{param.Type.ToString().ToLower()} {param.Name}{(i != nfVal.Options.Parameters.Count - 1 ? ", " : "")}";
                    }

                    if (nfVal.Options.AllParamsOfType != null)
                    {
                        done += $"{nfVal.Options.AllParamsOfType?.ToString().ToLower()}...";
                    }
                    else if (nfVal.Options.UncheckedParameters)
                    {
                        done += "?...?";
                    }

                    done += ");";
                    break;
            }

            return done;
        }
    }
}
