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
        public static RuntimeValue EvaluateExportStatement(ExportStatement statement, Environment environment)
        {
            RuntimeValue runtimeValue = Interpreter.Evaluate(statement.ToExport, environment);
            environment.ExportedVariables.Add(((Identifier)statement.ExportAs).Symbol, runtimeValue);
            Debug.Log($"Exported {((Identifier)statement.ExportAs).Symbol}", "export - " + statement.Location?.FileName);
            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
