﻿using System;
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
            { "let", TokenType.Let },
            { "const", TokenType.Const },
            { "export", TokenType.Export },
            { "function", TokenType.Function },
            { "return", TokenType.Return },
            { "if", TokenType.If },
            { "while", TokenType.While },
            { "for", TokenType.For },
            { "else", TokenType.Else },
            { "try", TokenType.Try },
            { "catch", TokenType.Catch },
            { "import", TokenType.Import },
            { "as", TokenType.As },
        };

        public static Dictionary<string, Runtime.Values.ValueType> Types = new()
        {
            { "int", Runtime.Values.ValueType.Int },
            { "long", Runtime.Values.ValueType.Long },
            { "float", Runtime.Values.ValueType.Float },
            { "double", Runtime.Values.ValueType.Double },
            { "string", Runtime.Values.ValueType.String },
            { "any", Runtime.Values.ValueType.Any },
            { "auto", Runtime.Values.ValueType.Any },
            { "bool", Runtime.Values.ValueType.Boolean },
            { "function", Runtime.Values.ValueType.Function },
            { "object", Runtime.Values.ValueType.Object },
        };

        public static Dictionary<string, Runtime.Values.Modifier> Modifiers = new()
        {
            { "final", Runtime.Values.Modifier.Final }
        };
    }
}