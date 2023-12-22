using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer.Syntax
{
    internal class Operator
    {
        public string Symbol;
        public TokenType TokenType;

        public Operator(string symbol, TokenType tokenType)
        {
            Symbol = symbol;
            TokenType = tokenType;
        }
    }
}
