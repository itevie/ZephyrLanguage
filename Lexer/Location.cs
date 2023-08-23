using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer
{
    internal class Location
    {
        public int Line { get; set; } = 0;
        public int TokenStart { get; set; } = 0;
        public int TokenEnd { get; set; } = 0;
        public string Source { get; set; } = "";
        public string FileName { get; set; } = "";
        public bool TillEnd { get; set; } = false;
    }
}
