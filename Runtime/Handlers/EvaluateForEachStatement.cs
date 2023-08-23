using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateForEachStatement(ForEachStatement statement, Environment environment)
        {
            string identifier = ((Identifier)statement.Identifier).Symbol;
            Expression exprValue = statement.ValueToEnumerate;
            RuntimeValue value = Interpreter.Evaluate(exprValue, environment);

            EnumerableValue enumerableValue = Values.Helpers.Helpers.CreateEnumerableValue(value);

            foreach (RuntimeValue runtimeValue in enumerableValue.Values)
            {
                Environment scope = new Environment(environment);
                scope.DeclareVariable(identifier, runtimeValue, new VariableSettings()
                {
                    IsConstant = true
                }, statement.Identifier);

                Interpreter.Evaluate(statement.Body, scope);
            }

            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
