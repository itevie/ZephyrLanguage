using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Parser.AST
{
    internal enum Kind
    {
        Unknown,

        Program,

        // ----- Statements -----
        VariableDeclaration,
        FunctionDeclaration,
        IfStatement,
        BlockStatement,
        LoopStatement,
        BreakStatement,
        ContinueStatement,
        ReturnStatement,
        EchoStatement,
        ApplyModifierStatement,
        ImportStatement,
        StructDeclaration,
        ForEachStatement,
        ForStatement,
        TryStatement,
        ThrowStatement,
        DecoratorApplierStatement,
        ExportStatement,

        // ----- Expressions -----
        BinaryExpression,
        AssignmentExpression,
        LogicalExpression,
        ComparisonExpression,
        TernaryExpression,
        RangeExpression,
        CastExpression,
        UnaryExpression,
        UnaryRightExpression,
        CallExpression,
        InExpression,
        MemberExpression,
        PipeExpression,
        AwaitExpression,
        FunctionExpression,
        LambdaExpression,
        LambdaArgumentList,
        SpreadExpression,

        // ----- Literals -----
        Identifier,
        NumericLiteral,
        StringLiteral,
        ArrayLiteral,
        ObjectLiteral,
    }
}
