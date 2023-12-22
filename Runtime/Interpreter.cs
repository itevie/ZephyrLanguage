using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;

namespace ZephyrNew.Runtime
{
    internal class Interpreter
    {
        public static Values.RuntimeValue Evaluate(Expression astNode, Environment environment)
        {
            try
            {
                Values.RuntimeValue value = astNode.Kind switch
                {
                    // ----- Statements -----
                    Kind.Program => Handlers.Statements.EvaluateProgram((Parser.AST.Program)astNode, environment),
                    Kind.EchoStatement => Handlers.Statements.EvaluateEchoStatement((EchoStatement)astNode, environment),
                    Kind.IfStatement => Handlers.Statements.EvaluateIfStatement((IfStatement)astNode, environment),
                    Kind.BlockStatement => Handlers.Statements.EvaluateBlockStatement((BlockStatement)astNode, environment),
                    Kind.LoopStatement => Handlers.Statements.EvaluateLoopStatement((LoopStatement)astNode, environment),
                    Kind.BreakStatement => Handlers.Statements.EvaluateBreakStatement((BreakStatement)astNode, environment),
                    Kind.ContinueStatement => Handlers.Statements.EvaluateContinueStatement((ContinueStatement)astNode, environment),
                    Kind.ReturnStatement => Handlers.Statements.EvaluateReturnStatement((ReturnStatement)astNode, environment),
                    Kind.VariableDeclaration => Handlers.Statements.EvaluateVariableDeclaration((VariableDeclaration)astNode, environment),
                    Kind.FunctionDeclaration => Handlers.Statements.EvaluateFunctionDeclaration((FunctionDeclaration)astNode, environment),
                    Kind.ApplyModifierStatement => Handlers.Statements.EvaluateApplyModifier((ApplyModifierStatement)astNode, environment),
                    Kind.ImportStatement => Handlers.Statements.EvaluateImportStatement((ImportStatement)astNode, environment),
                    Kind.ExportStatement => Handlers.Statements.EvaluateExportStatement((ExportStatement)astNode, environment),
                    Kind.StructDeclaration => Handlers.Statements.EvaluateStructDeclaration((StructDeclaration)astNode, environment),
                    Kind.ForEachStatement => Handlers.Statements.ParseForEachStatement((ForEachStatement)astNode, environment),
                    Kind.ForStatement => Handlers.Statements.EvaluateForStatement((ForStatement)astNode, environment),
                    Kind.TryStatement => Handlers.Statements.EvaluateTryStatement((TryStatement)astNode, environment),
                    Kind.DecoratorApplierStatement => Handlers.Statements.EvaluateDecoratorStatement((DecoratorApplierStatement)astNode, environment),

                    // ----- Expressions -----
                    Kind.InExpression => Handlers.Expressions.EvaluateInExpression((InExpression)astNode, environment),
                    Kind.UnaryExpression => Handlers.Expressions.EvaluateUnaryExpression((UnaryExpression)astNode, environment),
                    Kind.UnaryRightExpression => Handlers.Expressions.EvaluateUnaryRightExpression((UnaryRightExpression)astNode, environment),
                    Kind.CastExpression => Handlers.Expressions.EvaluateCastExpression((CastExpression)astNode, environment),
                    Kind.BinaryExpression => Handlers.Expressions.EvaluateBinaryExpression((BinaryExpression)astNode, environment),
                    Kind.RangeExpression => Handlers.Expressions.EvaluateRangeExpression((RangeExpression)astNode, environment),
                    Kind.TernaryExpression => Handlers.Expressions.EvaluateTernaryExpression((TernaryExpression)astNode, environment),
                    Kind.ComparisonExpression => Handlers.Expressions.EvaluateComparisonExpression((ComparisonExpression)astNode, environment),
                    Kind.LogicalExpression => Handlers.Expressions.EvaluateLogicalExpression((LogicalExpression)astNode, environment),
                    Kind.AssignmentExpression => Handlers.Expressions.EvaluateAssignmentExpression((AssignmentExpression)astNode, environment),
                    Kind.CallExpression => Handlers.Expressions.EvaluateCallExpression((CallExpression)astNode, environment),
                    Kind.MemberExpression => Handlers.Expressions.EvaluateMemberExpression((MemberExpression)astNode, environment),
                    Kind.PipeExpression => Handlers.Expressions.EvaluatePipeExpression((PipeExpression)astNode, environment),
                    Kind.AwaitExpression => Handlers.Expressions.EvaluateAwaitExpression((AwaitExpression)astNode, environment),
                    Kind.FunctionExpression => Handlers.Expressions.EvaluateFunctionExpression((FunctionExpression)astNode, environment),
                    Kind.LambdaExpression => Handlers.Expressions.EvaluateLambdaExpression((LambdaExpression)astNode, environment),
                    Kind.SpreadExpression => Handlers.Expressions.EvaluateSpreadExpression((SpreadExpression)astNode, environment),

                    // ----- Literals -----
                    Kind.NumericLiteral => Handlers.Literals.EvaluateNumericLiteral((NumericLiteral)astNode, environment),
                    Kind.Identifier => Handlers.Literals.EvaluateIdentifier((Identifier)astNode, environment),
                    Kind.StringLiteral => Handlers.Literals.EvaluateStringLiteral((StringLiteral)astNode, environment),
                    Kind.ArrayLiteral => Handlers.Literals.EvaluateArrayLiteral((ArrayLiteral)astNode, environment),
                    Kind.ObjectLiteral => Handlers.Literals.ParseObjectLiteral((ObjectLiteral)astNode, environment),

                    // ----- Unknown -----
                    _ => throw new RuntimeException($"The interpreter cannot evaluate this as it doesn't know how to handle the ast node: {astNode.Kind}", astNode.Location)
                };

                if (value.Location == Location.UnknownLocation)
                    value.Location = astNode.FullLocation ?? astNode.Location;
                return value;
            } catch (ZephyrException exception)
            {
                // Check if a location was provided which is not unknown
                if (exception.Location == Location.UnknownLocation)
                {
                    // Default to something that may be at least useful
                    exception.Location = astNode.Location;
                    exception.Location.AssumedLocation = true;
                }

                // Rethrow
                throw;
            }
        }
    }
}
