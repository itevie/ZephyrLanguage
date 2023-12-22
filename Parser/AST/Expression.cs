using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class Expression
    {
        public Kind Kind;
        public Location Location;
        public Location? FullLocation { get; set; } = null;

        public bool NeedsSemicolon { get; set; } = true;

        public Expression(Kind kind, Location location)
        {
            Kind = kind;
            Location = location;
        }
    }
}
