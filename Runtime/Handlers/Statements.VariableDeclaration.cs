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
        public static RuntimeValue EvaluateVariableDeclaration(VariableDeclaration declaration, Environment environment)
        {
            // Check if there was a value
            RuntimeValue value = declaration.Value == null ? new NullValue() : Interpreter.Evaluate(declaration.Value, environment);

            // Check for auto
            if (declaration.Type.TypeName == Values.ValueType.Auto)
            {
                declaration.Type.TypeName = value.Type.TypeName;
            }

            Variable variable = environment.DeclareVariable(
                declaration.Identifier.Symbol, 
                value, 
                new VariableSettings(
                    new VariableType(declaration.Type, environment, declaration.Type.Location), 
                    declaration.Identifier.Location
                ) { }, 
                declaration.Identifier.Location
            );

            //value.Type = new VariableType(declaration.Type, environment, declaration.Type.Location);

            return new VariableReference(variable);
        }
    }
}
