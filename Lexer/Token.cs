using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer.Syntax;

namespace Zephyr.Lexer
{
    internal class Token
    {
        public string Value;
        public TokenType TokenType;
        public Location Location;

        public Token(string value, TokenType tokenType, Location location)
        {
            Value = value;
            TokenType = tokenType;
            Location = location;
        }
    }
}
