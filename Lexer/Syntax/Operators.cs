using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Lexer.Syntax
{
    internal class Operators
    {
        public static Dictionary<string, Operator> ArithmeticOperators = new Dictionary<string, Operator>()
        {
            { "Plus", new Operator("+", TokenType.BinaryOperator) },
            { "Minus", new Operator("-", TokenType.BinaryOperator) },
            { "Multiply", new Operator("*", TokenType.BinaryOperator) },
            { "Divide", new Operator("/", TokenType.BinaryOperator) },
            { "Power", new Operator("^", TokenType.BinaryOperator) },
            { "Exponent", new Operator("**", TokenType.BinaryOperator) },
            { "Modulo", new Operator("%", TokenType.BinaryOperator) },
            { "Percentage", new Operator("%%", TokenType.BinaryOperator) },
            { "ReversePercentage", new Operator("%%?", TokenType.BinaryOperator) },
        };

        public static Dictionary<string, Operator> ComparisonOperators = new Dictionary<string, Operator>()
        {
            { "Equals", new Operator("==", TokenType.ComparisonOperator) },
            { "NotEquals", new Operator("!=", TokenType.ComparisonOperator) },
            { "GreaterThan", new Operator(">", TokenType.ComparisonOperator) },
            { "GreaterThanOrEquals", new Operator(">=", TokenType.ComparisonOperator) },
            { "LessThan", new Operator("<", TokenType.ComparisonOperator) },
            { "LessThanOrEquals", new Operator("<=", TokenType.ComparisonOperator) },
            { "MultiCompare", new Operator("<=>", TokenType.ComparisonOperator) },
        };

        public static Dictionary<string, Operator> LogicalOperators = new()
        {
            { "And", new Operator("and", TokenType.LogicalOperator) },
            { "Nand", new Operator("nand", TokenType.LogicalOperator) },
            { "Or", new Operator("or", TokenType.LogicalOperator) },
            { "Nor", new Operator("nor", TokenType.LogicalOperator) },
            { "Not", new Operator("!", TokenType.UnaryOperator) },
            { "XOR", new Operator("xor", TokenType.LogicalOperator) }
        };

        public static Dictionary<string, Operator> AssignmentOperators = new Dictionary<string, Operator>()
        {
            { "NormalAssignment", new Operator("=", TokenType.AssignmentOperator) }
        };

        public static Dictionary<string, Operator> BasicOperators = new Dictionary<string, Operator>()
        {
            { "QuestionMark", new Operator("?", TokenType.QuestionMark) },
            { "Comma", new Operator(",", TokenType.Comma) },
            { "Dot", new Operator(".", TokenType.Dot) },
            { "Colon", new Operator(":", TokenType.Colon) },
            { "Range", new Operator("..", TokenType.Range) },
            { "RangeUninclusive", new Operator(".<", TokenType.RangeUninclusive) },
            { "Step", new Operator("step", TokenType.Step) },
            { "LengthOf", new Operator("$", TokenType.UnaryOperator) },
            { "Increment", new Operator("++", TokenType.UnaryOperator) },
            { "Decrement", new Operator("--", TokenType.UnaryOperator) },
            { "Cast", new Operator("->", TokenType.CastOperator) },
            { "Typeof", new Operator("typeof", TokenType.UnaryOperator) },
            { "In", new Operator("in", TokenType.In) },
            { "Pipe", new Operator("|>", TokenType.Pipe) },
            { "Lambda", new Operator("=>", TokenType.Lambda) },
            { "Spread", new Operator("...", TokenType.Spread) },
            { "Decorator", new Operator("@", TokenType.Decorator) },
                
            // ----- Brackets -----
            { "OpenSquare", new Operator("[", TokenType.OpenSquare) },
            { "CloseSquare", new Operator("]", TokenType.CloseSquare) },
            { "OpenBrace", new Operator("{", TokenType.OpenBrace) },
            { "CloseBrace", new Operator("}", TokenType.CloseBrace) },
            { "OpenParenethesis", new Operator("(", TokenType.OpenParenthesis) },
            { "CloseParenthesis", new Operator(")", TokenType.CloseParenthesis) },
            { "OpenPipe", new Operator("|", TokenType.OpenPipe) },

        };

        public static Operator GetOperatorByName(string name)
        {
            if (!AllOperators.ContainsKey(name))
                throw new ZephyrException($"The operator {name} was not found", Location.UnknownLocation);
            return AllOperators[name];
        }

        // List of all operators for lexing
        public static Dictionary<string, Operator> OperatorsContainingLetters = new Dictionary<string, Operator>();
        public static Dictionary<string, Operator> AllOperators = GetAllOperators();

        private static Dictionary<string, Operator> GetAllOperators()
        {
            Dictionary<string, Operator> allOperators = new Dictionary<string, Operator>();

            Dictionary<string, Operator>[] operatorDictionaries =
            {
                ArithmeticOperators,
                AssignmentOperators,
                LogicalOperators,
                ComparisonOperators,
                BasicOperators,
            };

            // Add all the operators to finished
            foreach (Dictionary<string, Operator> current in operatorDictionaries)
            {
                foreach (KeyValuePair<string, Operator> oper in current)
                {
                    // Check if it contains a letter
                    if (oper.Value.Symbol.Any(x => char.IsLetter(x)))
                        OperatorsContainingLetters.Add(oper.Key, oper.Value);
                    else allOperators.Add(oper.Key, oper.Value);
                }
            }

            // Sort by reversed length (long -> short)
            var sorted = allOperators.OrderBy(key => key.Value.Symbol.Length).Reverse();

            // Finalise
            Dictionary<string, Operator> finished = new Dictionary<string, Operator>();

            foreach (var oper in sorted)
            {
                finished.Add(oper.Key, oper.Value);
            }

            return finished;
        }
    }
}
