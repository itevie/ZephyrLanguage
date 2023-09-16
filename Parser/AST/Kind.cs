﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    /// <summary>
    /// All AST nodes will reference this
    /// </summary>
    internal enum Kind
    {
        Unknown,

        Program,

        ExportStatement,
        VariableDeclaration,
        EventDeclaration,
        Modifier,
        FunctionDeclaration,
        ReturnStatement,
        BreakStatement,
        ContinueStatement,
        IfStatement,
        BlockStatement,
        WhileStatement,
        ForEachStatement,
        TryStatement,
        ImportStatement,

        AssignmentExpression,
        MemberExpression,
        CallExpression,
        LogicalExpression,
        ComparisonExpression,
        UnaryExpression,
        UnaryRightExpression,
        IndexerExpression,
        RangeExpression,
        CastExpression,
        TernaryExpression,
        PipeExpression,

        Property,
        ObjectLiteral,
        NumericLiteral,
        Identifier,
        BinaryExpression,
        StringLiteral,
        ArrayLiteral,
        Varref,
    }
}
