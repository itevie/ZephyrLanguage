using System;
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

        // Statements
        ExportStatement,
        VariableDeclaration,
        EventDeclaration,
        Modifier,
        FunctionDeclaration,
        ReturnStatement,
        BreakStatement,
        PassthroughStatement,
        ContinueStatement,
        IfStatement,
        BlockStatement,
        WhileStatement,
        ForEachStatement,
        TryStatement,
        ImportStatement,
        SwitchStatement,

        // Expression
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

        // Literals
        Property,
        ObjectLiteral,
        NumericLiteral,
        Identifier,
        BinaryExpression,
        StringLiteral,
        ArrayLiteral,
        Varref,

        // Other
        SwitchCase,
    }
}
