using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;
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
            switch (astNode.Kind)
            {
                // Statements
                case Kind.Program:
                    return Statements.EvaluateProgram((Parser.AST.Program)astNode, environment);
                case Kind.VariableDeclaration:
                    return Statements.EvaluateVariableDeclaration((VariableDeclaration)astNode, environment);
                case Kind.IfStatement:
                    return Statements.EvaluateIfStatement((IfStatement)astNode, environment);
                case Kind.BlockStatement:
                    return Statements.EvaluateBlockStatement((BlockStatement)astNode, environment);
                case Kind.FunctionDeclaration:
                    return Statements.EvaluateFunctionDeclaration((FunctionDeclaration)astNode, environment);
                case Kind.ReturnStatement:
                    return Statements.EvaluateRuntimeStatment((ReturnStatement)astNode, environment);
                case Kind.WhileStatement:
                    return Statements.EvaluateWhileStatement((WhileStatement)astNode, environment);
                case Kind.ImportStatement:
                    return Statements.EvaluateImportStatement((ImportStatement)astNode, environment);
                case Kind.ExportStatement:
                    return Statements.EvaluateExportStatement((ExportStatement)astNode, environment);
                case Kind.ForEachStatement:
                    return Statements.EvaluateForEachStatement((ForEachStatement)astNode, environment);
                case Kind.TryStatement:
                    return Statements.EvaluateTryStatement((TryStatement)astNode, environment);

                // Literals
                case Kind.Identifier:
                    return Expressions.EvaluateIdentifier((Identifier)astNode, environment);
                case Kind.NumericLiteral:
                    return Expressions.EvaluateNumericLiteral((NumericLiteral)astNode, environment);
                case Kind.StringLiteral:
                    return Expressions.EvaluateStringLiteral((StringLiteral)astNode, environment);
                case Kind.ArrayLiteral:
                    return Expressions.EvaluateArrayLiteral((ArrayLiteral)astNode, environment);
                case Kind.ObjectLiteral:
                    return Expressions.EvaluateObjectLiteral((ObjectLiteral)astNode, environment);

                // Expressions
                case Kind.BinaryExpression:
                    return Expressions.EvaluateBinaryExpression((BinaryExpression)astNode, environment);
                case Kind.LogicalExpression:
                    return Expressions.EvaluateLogicalExpression((LogicalExpression)astNode, environment);
                case Kind.UnaryRightExpression:
                    return Expressions.EvaluateUnaryRightExpression((UnaryRightExpression)astNode, environment);
                case Kind.UnaryExpression:
                    return Expressions.EvaluateUnaryExpression((UnaryExpression)astNode, environment);
                case Kind.CallExpression:
                    return Expressions.EvaluateCallExpression((CallExpression)astNode, environment);
                case Kind.MemberExpression:
                    return Expressions.EvaluateMemberExpression((MemberExpression)astNode, environment);
                case Kind.AssignmentExpression:
                    return Expressions.EvaluateAssignmentExpression((AssignmentExpression)astNode, environment);
                case Kind.ComparisonExpression:
                    return Expressions.EvaluateComparisonExpression((ComparisonExpression)astNode, environment);
                case Kind.RangeExpression:
                    return Expressions.EvaluateRangeExpression((RangeExpression)astNode, environment);
                case Kind.CastExpression:
                    return Expressions.EvaluateCastExpression((CastExpression)astNode, environment);
                case Kind.TernaryExpression:
                    return Expressions.EvaluateTernaryExpression((TernaryExpression)astNode, environment);
                case Kind.Varref:
                    return Expressions.EvaluateVarref((VarrefExpression)astNode, environment);

                // AST node's kind is unknown and cannot be computed
                default:
                    throw new Exception($"The interpreter cannot handle this ast node because it cannot handle a {astNode.Kind}");
            }
        }
    }
}
