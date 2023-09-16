using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Lexer.Syntax
{
    internal class Operators
    {
        // Unary operators
        public static Dictionary<string, Operator> UnaryOperators = new()
        {
            { "LengthOf", new Operator("$", TokenType.UnaryOperator) },
            { "Increment", new Operator("++", TokenType.UnaryOperator) },
            { "Decrement", new Operator("--", TokenType.UnaryOperator) },
            { "Dereferencer", new Operator("@", TokenType.UnaryOperator) },
            { "Referencer", new Operator("&", TokenType.UnaryOperator) },
        };

        // Arithmetic operators
        public static Dictionary<string, Operator> ArithmeticOperators = new()
        {
            { "Plus", new Operator("+", TokenType.BinaryOperator) },
            { "Subtract", new Operator("-", TokenType.BinaryOperator) },
            { "Divide", new Operator("/", TokenType.BinaryOperator) },
            { "Modulus", new Operator("%", TokenType.BinaryOperator) },
            { "Multiply", new Operator("*", TokenType.BinaryOperator) },
            { "Power", new Operator("**", TokenType.BinaryOperator) },
        };

        // Other binary operators
        public static Dictionary<string, Operator> BinaryOperators = new()
        {
            { "Concat", new Operator("~", TokenType.BinaryOperator) },
            { "Coalesence", new Operator("??", TokenType.BinaryOperator) },
            { "Pipe", new Operator(">>", TokenType.Pipe) },
        };

        // Syntax sugar
        /*public static Dictionary<string, OperatingSystem> SugarOperators = new()
        {
            { "ForEachIn", new Operator("::", TokenType.) }
        }*/

        // Comparison operators
        public static Dictionary<string, Operator> ComparisonOperators = new()
        {
            { "Equals", new Operator("==", TokenType.ComparisonOperator) },
            { "NotEquals", new Operator("!=", TokenType.ComparisonOperator) },
            { "GreaterThan", new Operator(">", TokenType.ComparisonOperator) },
            { "GreaterThanOrEquals", new Operator(">=", TokenType.ComparisonOperator) },
            { "LessThan", new Operator("<", TokenType.ComparisonOperator) },
            { "LessThanOrEquals", new Operator("<=", TokenType.ComparisonOperator) },
            { "MultiCompare", new Operator("<=>", TokenType.ComparisonOperator) },
            { "In", new Operator("=>?", TokenType.ComparisonOperator) },
            { "NotIn", new Operator("!=>?", TokenType.ComparisonOperator) },
        };

        // Assignment operators
        public static Dictionary<string, Operator> AssignmentOperators = new()
        {
            { "NormalAssignment", new Operator("=", TokenType.AssignmentOperator) },
            { "PlusAssignment", new Operator("+=", TokenType.AssignmentOperator) },
            { "SubtractAssignment", new Operator("-=", TokenType.AssignmentOperator) },
            { "DivideAssignment", new Operator("/=", TokenType.AssignmentOperator) },
            { "ModulusAssignment", new Operator("%=", TokenType.AssignmentOperator) },
            { "MultiplyAssignment", new Operator("*=", TokenType.AssignmentOperator) },
            { "PowerAssignment", new Operator("**=", TokenType.AssignmentOperator) },
            { "CoalesenceAssignment", new Operator("??=", TokenType.AssignmentOperator) },
            { "ConcatAssignment", new Operator("~=", TokenType.AssignmentOperator) },
        };

        // Logical operators
        /*public static Dictionary<string, Operator> LogicalOperators = new()
        {
            { "And", new Operator("&&", TokenType.LogicalOperator) },
            { "Or", new Operator("||", TokenType.LogicalOperator) },
            { "Not", new Operator("!", TokenType.UnaryOperator) },
            { "XOR", new Operator("|+|", TokenType.LogicalOperator) }
        };*/

        public static Dictionary<string, Operator> LogicalOperators = new()
        {
            { "And", new Operator("and", TokenType.LogicalOperator) },
            { "Or", new Operator("or", TokenType.LogicalOperator) },
            { "Not", new Operator("!", TokenType.UnaryOperator) },
            { "XOR", new Operator("xor", TokenType.LogicalOperator) }
        };

        // Single ones
        public static Dictionary<string, Operator> SingleOperators = new()
        {
            { "QuestionMark", new Operator("?", TokenType.QuestionMark) },
            { "Colon", new Operator(":", TokenType.Colon) },
            { "ForEachIn", new Operator("in", TokenType.ForEachIn) },
            { "Cast", new Operator("->", TokenType.Cast) },
            { "DoubleDot", new Operator("..", TokenType.DoubleDot) },
            { "DoubleDotUninclusive", new Operator(".<", TokenType.DoubleDotUninclusive) },
            { "Dot", new Operator(".", TokenType.Dot) }
        };

        // Special
        public static char SpecialIdentifierPrefix = '#';

        // List of all the above operators
        public static Dictionary<string, Operator> LetterContainingOperators = new();
        public static Dictionary<string, Operator> AllOperators = GetAllOperators();

        private static Dictionary<string, Operator> GetAllOperators()
        {
            Dictionary<string, Operator> tokens = new();

            Dictionary<string, Operator>[] operators =
            {
                UnaryOperators,
                ArithmeticOperators,
                AssignmentOperators,
                ComparisonOperators,
                BinaryOperators,
                LogicalOperators,
                SingleOperators,
            };

            // Add all the operators
            foreach (Dictionary<string, Operator> operatorList in operators)
            {
                foreach (KeyValuePair<string, Operator> token in operatorList)
                {
                    // Check if contains a letter
                    if (token.Value.Symbol.Any(x => char.IsLetter(x)))
                    {
                        LetterContainingOperators.Add(token.Key, token.Value);
                    } else tokens.Add(token.Key, token.Value);
                }
            }

            // Sort it
            var t = tokens.OrderBy(key => key.Value.Symbol.Length).Reverse();

            Dictionary<string, Operator> returning = new();

            foreach (var tok in t)
            {
                returning.Add(tok.Key, tok.Value);
            }

            return returning;
        }
    }
}
