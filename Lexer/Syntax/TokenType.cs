using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer.Syntax
{
    internal enum TokenType
    {
        // Literal Types
        Number,
        Identifier,
        SpecialIdentifier,
        String,
        Type,

        // Keywords
        Let,
        Const,
        Export,
        Function,
        Struct,
        Return,
        If,
        While,
        For,
        Else,
        Try,
        Catch,
        Import,
        As,
        Varref,

        Modifier,

        // Basic Syntax
        Comma,
        Dot,
        DoubleDot, 
        DoubleDotUninclusive,
        Colon,
        Semicolon,
        QuestionMark,
        ForEachIn,
        Cast,
        EOF,

        // Operators
        UnaryOperator,
        BinaryOperator,
        LogicalOperator,
        ComparisonOperator,
        AssignmentOperator,
        BitwiseOperator,

        // Brackets
        OpenParan,
        CloseParan,
        OpenBrace,
        CloseBrace,
        OpenSquare,
        CloseSquare
    }
}
