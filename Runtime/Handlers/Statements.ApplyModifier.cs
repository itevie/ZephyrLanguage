using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateApplyModifier(ApplyModifierStatement statement, Environment environment)
        {
            RuntimeValue value = Interpreter.Evaluate(statement.Value, environment);

            // Expect a variable reference
            if (value.Type.TypeName != Values.ValueType.VariableReference)
            {
                throw new RuntimeException($"Can only apply modifiers to a VariableReference", statement.Value.Location);
            }

            VariableReference varRef = (VariableReference)value;

            // Check if value already has the modifier
            if (varRef.Variable.Value.HasModifier(statement.Modifier))
            {
                throw new RuntimeException($"The variable already has this modifier {statement.Modifier}", statement.Value.Location);
            }

            // Add modifier
            varRef.Variable.Value.AddModifier(statement.Modifier);

            // Done
            return varRef;
        }
    }
}
