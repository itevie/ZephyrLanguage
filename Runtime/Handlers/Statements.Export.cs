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
        public static RuntimeValue EvaluateExportStatement(ExportStatement statement, Environment environment)
        {
            // Check if the toExport is an identifier
            if (statement.ToExport.Kind == Kind.Identifier)
            {
                RuntimeValue value = environment.LookupVariable(((Identifier)statement.ToExport).Symbol, statement.ToExport.Location);
                environment.ExportVariable(((Identifier)statement.ToExport).Symbol, value, statement.ToExport.Location);
                return new NullValue();
            }

            RuntimeValue toExport = Interpreter.Evaluate(statement.ToExport, environment);

            switch (toExport.Type.TypeName)
            {
                case Values.ValueType.VariableReference:
                    VariableReference vrv = (VariableReference)toExport;
                    environment.ExportVariable(vrv.Variable.Settings.Name, vrv.Variable.Value, statement.ToExport.Location);
                    break;
                case Values.ValueType.Object:
                    ObjectValue ov = (ObjectValue)toExport;

                    foreach (KeyValuePair<string, RuntimeValue> kv in ov.Properties)
                    {
                        environment.ExportVariable(kv.Key, kv.Value, kv.Value.Location);
                    }
                    break;
                case Values.ValueType.Module:
                    ModuleValue mv = (ModuleValue)toExport;

                    foreach (KeyValuePair<string, RuntimeValue> kv in mv.Variables)
                    {
                        environment.ExportVariable(kv.Key, kv.Value, kv.Value.Location);
                    }
                    break;
                default:
                    throw new RuntimeException($"Cannot export {statement.ToExport.Kind}", statement.ToExport.Location);
            }

            return new NullValue();
        }
    }
}
