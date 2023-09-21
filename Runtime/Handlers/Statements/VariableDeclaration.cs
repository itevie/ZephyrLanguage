using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static Values.RuntimeValue EvaluateVariableDeclaration(Parser.AST.VariableDeclaration declaration, Environment environment)
        {
            Values.RuntimeValue value = declaration.Value != null
                ? Interpreter.Evaluate(declaration.Value, environment)
                : Values.Helpers.Helpers.CreateNull();

            if (declaration.Type == Values.ValueType.Auto)
            {
                declaration.Type = value.Type;
            }

            // Cast for numbers
            if (value.Type == Values.ValueType.Int || value.Type == Values.ValueType.Long || value.Type == Values.ValueType.Float)
                value = Helpers.CastValueHelper(value, declaration.Type, Helpers.GetLocation(declaration.Value.Location));

            value.Modifiers = declaration.Modifiers;
            value.DeclaredAt = declaration;

            return environment.DeclareVariable(declaration.Identifier.Symbol, value, new VariableSettings()
            {
                IsConstant = declaration.IsConstant,
                Type = declaration.Type,
                DeclaredAt = declaration,
                IsNullable = declaration.IsTypeNullable,
            }, Helpers.GetLocation(declaration.Identifier, declaration, declaration.Value));
        }
    }
}
