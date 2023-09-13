using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer
{
    /// <summary>
    /// This is used everywhere to indicate where the source originated from, for example, where an identifier is in the file
    /// </summary>
    internal class Location
    {
        /// <summary>
        /// The line on which the item resides
        /// </summary>
        public int Line { get; set; } = 0;

        /// <summary>
        /// Where the token starts in the line
        /// </summary>
        public int TokenStart { get; set; } = 0;

        /// <summary>
        /// Where the token ends in the line
        /// </summary>
        public int TokenEnd { get; set; } = 0;

        /// <summary>
        /// The complete source of the variable
        /// Remove this at some point it might be bad i think
        /// </summary>
        public string Source { get; set; } = "";

        /// <summary>
        /// The file name - this is purely only for showing which file name it is from
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// Whether or not the token goes off onto the next line, if it does instead of showing it, show ^...
        /// </summary>
        public bool TillEnd { get; set; } = false;
    }
}
