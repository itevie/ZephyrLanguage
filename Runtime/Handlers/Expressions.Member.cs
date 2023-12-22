using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateMemberExpression(Parser.AST.MemberExpression expression, Environment environment, RuntimeValue? setTo = null, Location? setToLocation = null)
        {
            RuntimeValue left = Interpreter.Evaluate(expression.Left, environment);

            string identifier;

            if (expression.IsComputed)
            {
                RuntimeValue right = Interpreter.Evaluate(expression.Right, environment);

                // Check if is is a number and accessing an array
                if (left.Type.TypeName == Values.ValueType.Array && right.Type.TypeName == Values.ValueType.Number)
                {
                    ArrayValue arr = (ArrayValue)left;
                    int index = (int)((NumberValue)right).Value;
                    if (setTo != null)
                    {
                        // Check final
                        if (arr.HasModifier(Modifier.Final))
                            throw new RuntimeException($"Cannot assign to array as it is marked as final", expression.Left.Location);

                        Values.Helpers.CanAddToArray(arr, setTo);
                        if (arr.Items.Count < index)
                            throw new RuntimeException($"Array index out of bounds", setToLocation);
                        if (arr.Items.Count == index)
                            arr.Items.Add(setTo);
                        else arr.Items[index] = setTo;
                        return new NullValue();
                    }

                    if (arr.Items.Count - 1 < index)
                        return new NullValue();
                    return arr.Items[index];
                }

                // Expect string
                if (right.Type.TypeName != Values.ValueType.String)
                    throw new RuntimeException($"Expected right side to be of type string", expression.Right.Location);

                identifier = ((StringValue)right).Value;
            } else
            {
                identifier = ((Identifier)expression.Right).Symbol;
            }

            // Check if it is a module
            if (left.Type.TypeName == Values.ValueType.Module)
            {
                ModuleValue l = (ModuleValue)left;
                if (!l.Variables.ContainsKey(identifier))
                    throw new RuntimeException($"Module does not contain definition for {identifier}", expression.Right.Location);
                return l.Variables[identifier];
            }

            if (setTo != null)
            {
                // Check final
                if (left.HasModifier(Modifier.Final))
                    throw new RuntimeException($"Cannot assign to object as it is marked as final", expression.Left.Location);

                left.SetProperty(identifier, setTo, setToLocation);
                return new NullValue();
            }

            return left.GetProperty(identifier, expression.Right.Location);
        }
    }
}
