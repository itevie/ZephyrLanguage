using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer.Syntax
{
    internal class Prefix
    {
        public string Symbol;
        public PrefixType PrefixType;

        public Prefix(string symbol, PrefixType prefixType)
        {
            Symbol = symbol;
            PrefixType = prefixType;
        }
    }
}
