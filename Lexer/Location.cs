using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer
{
    internal class Location
    {
        public int Line { get; set; } = 0;
        public int TokenStart { get; set; } = 0;
        public int TokenEnd { get; set; } = 0;
        public string FileName { get; set; } = "<Unknown Filename>";
        public bool TillEnd { get; set; } = false;
        public string? Source { get; set; } = null;
        public bool AssumedLocation { get; set; } = false;

        public string GenerateSimple()
        {
            // Compute where it happened
            int fromChar;
            int toChar = -1;

            if (TokenStart == TokenEnd || (TokenEnd == TokenStart + 1))
                fromChar = TokenStart + 1;
            else
            {
                fromChar = TokenStart + 1;
                toChar = TokenEnd;
            }

            // Add where it happened
            return  $"{FileName}:[Line {Line + 1} Char {fromChar}{(toChar != -1 ? "-" + toChar : "")}]{(AssumedLocation ? " (Assumed)" : "")}";
        }

        // ----- Static Presets -----
        public static Location UnknownLocation = new Location();
    }
}
