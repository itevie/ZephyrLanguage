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

            value.Modifiers = declaration.Modifiers;

            return environment.DeclareVariable(declaration.Identifier.Symbol, value, new VariableSettings()
            {
                IsConstant = declaration.IsConstant,
                Type = declaration.Type,
                IsNullable = declaration.IsTypeNullable,
            }, Helpers.GetLocation(declaration.Identifier, declaration, declaration.Value));
        }
    }
}
