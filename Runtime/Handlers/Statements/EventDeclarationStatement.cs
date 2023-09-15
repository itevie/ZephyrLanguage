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
        public static RuntimeValue EventDeclarationStatement(EventDeclarationStatement statement, Environment environment)
        {
            EventValue value = new EventValue()
            {
                EventName = statement.Identifier.Symbol,
                EventType = statement.Type.Type,
            };

            environment.DeclareVariable(statement.Identifier.Symbol, value, new()
            {
                Type = Values.ValueType.Event,
                Modifiers = new()
                {
                    Modifier.Final
                }
            });

            return value;
        }
    }
}
