using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime;

namespace ZephyrNew.Lexer.Syntax
{
    internal class Keywords
    {
        public static Dictionary<string, TokenType> KeywordList = new Dictionary<string, TokenType>()
        {
            { "var", TokenType.Var },
            { "struct", TokenType.Struct },
            { "fn", TokenType.Function},
            { "params", TokenType.Params },
            //{ "f", TokenType.FunctionExpression},
            { "return", TokenType.Return},
            { "if", TokenType.If },
            { "else", TokenType.Else },

            { "for", TokenType.For },
            { "loop", TokenType.Loop },
            { "break", TokenType.Break },
            { "continue", TokenType.Continue },

            { "try", TokenType.Try },
            { "catch", TokenType.Catch },
            { "throw", TokenType.Throw },


            { "from", TokenType.From },
            { "import", TokenType.Import},
            { "export", TokenType.Export },
            { "as", TokenType.As},

            { "echo", TokenType.Echo },

            { "await", TokenType.Await },
        };

        public static Dictionary<string, ValueType> Types = new Dictionary<string, ValueType>()
        {
            { "number", Runtime.Values.ValueType.Number },
            //{ "long", Runtime.Values.ValueType.Long },
            //{ "float", Runtime.Values.ValueType.Integer },
            //{ "double", Runtime.Values.ValueType.Double },
            { "string", Runtime.Values.ValueType.String },
            { "bool", Runtime.Values.ValueType.Boolean },
            { "any", Runtime.Values.ValueType.Any },
            { "object", Runtime.Values.ValueType.Object },
            { "module", Runtime.Values.ValueType.Module },
            { "void", Runtime.Values.ValueType.Void },
            { "Fn", Runtime.Values.ValueType.Function },
        };

        public static Dictionary<string, Modifier> Modifiers = new Dictionary<string, Modifier>()
        {
            { "final", Modifier.Final },
        };
    }
}
