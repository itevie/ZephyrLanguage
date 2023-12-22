using ZephyrNew.Lexer.Syntax;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateAssignmentExpression(AssignmentExpression expression, Environment environment)
        {
            // Check if it is MemberExpression
            if (expression.Assignee.Kind == Kind.MemberExpression)
            {
                return EvaluateMemberExpression(
                    (MemberExpression)expression.Assignee,
                    environment,
                    Interpreter.Evaluate(expression.Value, environment),
                    expression.Value.Location
                );
            }

            // Check if it is allowed to be assigned to
            if (expression.Assignee.Kind != Kind.Identifier)
                throw new RuntimeException($"Cannot assign to ${expression.Assignee.Kind}", expression.Assignee.Location);

            // Get the values
            string variableName = ((Identifier)expression.Assignee).Symbol;

            RuntimeValue givenNewValue = Interpreter.Evaluate(expression.Value, environment);

            RuntimeValue newValue = new Values.NullValue();
            RuntimeValue oldValue = environment.LookupVariable(variableName, expression.Assignee.Location);

            // Check the type
            if (expression.Operator == Operators.AssignmentOperators["NormalAssignment"].Symbol)
            {
                newValue = givenNewValue;
            }
            else
            {
                throw new RuntimeException($"Cannot use this operator", expression.Location);
            }

            return environment.AssignVariable(variableName, newValue, expression.Location);
        }
    }
}
