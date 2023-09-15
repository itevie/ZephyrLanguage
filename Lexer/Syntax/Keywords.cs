using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer.Syntax
{
    internal class Keywords
    {
        public static Dictionary<string, TokenType> TheKeywords = new()
        {
            { "var", TokenType.Let },
            { "const", TokenType.Const },
            { "export", TokenType.Export },
            { "func", TokenType.Function },
            { "struct", TokenType.Struct },
            { "return", TokenType.Return },
            { "if", TokenType.If },
            { "while", TokenType.While },
            { "for", TokenType.For },
            { "else", TokenType.Else },
            { "try", TokenType.Try },
            { "catch", TokenType.Catch },
            { "import", TokenType.Import },
            { "as", TokenType.As },
            { "varref", TokenType.Varref },
            { "break", TokenType.Break },
            { "continue", TokenType.Continue },
            { "event", TokenType.Event }
        };

        public static Dictionary<string, Runtime.Values.ValueType> Types = new()
        {
            { "int", Runtime.Values.ValueType.Int },
            { "long", Runtime.Values.ValueType.Long },
            { "float", Runtime.Values.ValueType.Float },
            //{ "double", Runtime.Values.ValueType.Double },
            { "string", Runtime.Values.ValueType.String },
            { "any", Runtime.Values.ValueType.Any },
            { "auto", Runtime.Values.ValueType.Auto },
            { "bool", Runtime.Values.ValueType.Boolean },
            { "func", Runtime.Values.ValueType.Function },
            { "object", Runtime.Values.ValueType.Object },
        };

        public static Dictionary<string, Runtime.Values.Modifier> Modifiers = new()
        {
            { "final", Runtime.Values.Modifier.Final }
        };
    }
}
