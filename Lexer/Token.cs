using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer
{
    /// <summary>
    /// Represents a lexer token
    /// </summary>
    internal class Token
    {
        public string Value;
        public string? StringValue = null;
        public TokenType Type;
        public Location Location;

        public Token(string value, TokenType type, Location location, string? stringValue = null)
        {
            Value = value;
            Type = type;
            Location = location;
            StringValue = stringValue;
        }
    }
}
