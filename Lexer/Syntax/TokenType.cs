using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer
{
    internal enum TokenType
    {
        // ----- Literals -----
        Identifier,
        Number,
        String,
        Type,
        Modifier,

        // ----- Operators -----
        BinaryOperator,
        UnaryOperator,
        LogicalOperator,
        AssignmentOperator,
        ComparisonOperator,
        CastOperator,

        // ----- Basic Syntax -----
        Semicolon,
        QuestionMark,
        Comma,
        Dot,
        Colon,
        Pipe,
        Spread,
        Decorator,

        // ----- Brackets -----
        OpenSquare,
        CloseSquare,
        OpenBrace,
        CloseBrace,
        OpenParenthesis,
        CloseParenthesis,
        OpenPipe,

        // ----- Keywords -----
        Var,
        Const,

        Function,
        Params,
        FunctionExpression,
        Lambda,
        Return,

        Struct,

        Await,

        If,
        Else,

        Try,
        Catch,
        Throw,

        Import,
        Export,
        From,
        As,

        Loop,
        For,
        While,
        Do,
        Break,
        Continue,

        Range,
        RangeUninclusive,
        Step,

        In,

        // ----- Special -----
        EOF,
        Echo,
    }
}
