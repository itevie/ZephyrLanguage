using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer.Syntax
{
    internal static class Prefixes
    {
        public static Prefix VerbatimString = new Prefix("v", PrefixType.Verbatim);
        public static Prefix VerbatimIdentifier = new Prefix("v#", PrefixType.Verbatim);
    }
}
