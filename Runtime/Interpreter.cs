using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Parser.AST.Expressions;
using Zephyr.Runtime.Handlers;

namespace Zephyr.Runtime
{
    /// <summary>
    /// This evaluates an AST node by checking it's kind and running the appropiate function
    /// </summary>
    internal class Interpreter
    {
        /// <summary>
        /// Evaluates the AST node
        /// </summary>
        /// <param name="astNode">The AST node to execute</param>
        /// <param name="environment">The environment in which to run it in</param>
        /// <returns>The returned value</returns>
        /// <exception cref="Exception"></exception>
        public static Values.RuntimeValue Evaluate(Expression astNode, Environment environment)
        {
            return astNode.Kind switch
            {
                // Statements
                Kind.Program => Statements.EvaluateProgram((Parser.AST.Program)astNode, environment),
                Kind.VariableDeclaration => Statements.EvaluateVariableDeclaration((VariableDeclaration)astNode, environment),
                Kind.EventDeclaration => Statements.EventDeclarationStatement((EventDeclarationStatement)astNode, environment),
                Kind.IfStatement => Statements.EvaluateIfStatement((IfStatement)astNode, environment),
                Kind.BlockStatement => Statements.EvaluateBlockStatement((BlockStatement)astNode, environment),
                Kind.FunctionDeclaration => Statements.EvaluateFunctionDeclaration((FunctionDeclaration)astNode, environment),
                Kind.ReturnStatement => Statements.EvaluateRuntimeStatment((ReturnStatement)astNode, environment),
                Kind.BreakStatement => Statements.EvaluateBreakStatement((BreakStatement)astNode, environment),
                Kind.WhileStatement => Statements.EvaluateWhileStatement((WhileStatement)astNode, environment),
                Kind.ImportStatement => Statements.EvaluateImportStatement((ImportStatement)astNode, environment),
                Kind.ExportStatement => Statements.EvaluateExportStatement((ExportStatement)astNode, environment),
                Kind.ForEachStatement => Statements.EvaluateForEachStatement((ForEachStatement)astNode, environment),
                Kind.TryStatement => Statements.EvaluateTryStatement((TryStatement)astNode, environment),

                // Literals
                Kind.Identifier => Expressions.EvaluateIdentifier((Identifier)astNode, environment),
                Kind.NumericLiteral => Expressions.EvaluateNumericLiteral((NumericLiteral)astNode, environment),
                Kind.StringLiteral => Expressions.EvaluateStringLiteral((StringLiteral)astNode, environment),
                Kind.ArrayLiteral => Expressions.EvaluateArrayLiteral((ArrayLiteral)astNode, environment),
                Kind.ObjectLiteral => Expressions.EvaluateObjectLiteral((ObjectLiteral)astNode, environment),

                // Expressions
                Kind.BinaryExpression => Expressions.EvaluateBinaryExpression((BinaryExpression)astNode, environment),
                Kind.LogicalExpression => Expressions.EvaluateLogicalExpression((LogicalExpression)astNode, environment),
                Kind.UnaryRightExpression => Expressions.EvaluateUnaryRightExpression((UnaryRightExpression)astNode, environment),
                Kind.UnaryExpression => Expressions.EvaluateUnaryExpression((UnaryExpression)astNode, environment),
                Kind.CallExpression => Expressions.EvaluateCallExpression((CallExpression)astNode, environment),
                Kind.MemberExpression => Expressions.EvaluateMemberExpression((MemberExpression)astNode, environment),
                Kind.AssignmentExpression => Expressions.EvaluateAssignmentExpression((AssignmentExpression)astNode, environment),
                Kind.ComparisonExpression => Expressions.EvaluateComparisonExpression((ComparisonExpression)astNode, environment),
                Kind.RangeExpression => Expressions.EvaluateRangeExpression((RangeExpression)astNode, environment),
                Kind.CastExpression => Expressions.EvaluateCastExpression((CastExpression)astNode, environment),
                Kind.TernaryExpression => Expressions.EvaluateTernaryExpression((TernaryExpression)astNode, environment),
                Kind.Varref => Expressions.EvaluateVarref((VarrefExpression)astNode, environment),
                Kind.PipeExpression => Expressions.EvaluatePipeExpression((PipeExpression)astNode, environment),

                // AST node's kind is unknown and cannot be computed
                _ => throw new Exception($"The interpreter cannot handle this ast node because it cannot handle a {astNode.Kind}"),
            };
        }
    }
}
