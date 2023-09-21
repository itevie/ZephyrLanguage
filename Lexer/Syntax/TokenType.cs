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
        Event,
        Const,
        Export,
        Function,
        Struct,
        Return,
        Break,
        Continue,
        If,
        While,
        For,
        Else,
        Try,
        Catch,
        Import,
        As,
        Varref,
        Switch,
        Case,
        Passthrough,

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
        Pipe,
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
