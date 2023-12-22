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
        public static RuntimeValue EvaluateStructDeclaration(StructDeclaration declaration, Environment environment)
        {
            Dictionary<string, VariableType> fields = new Dictionary<string, VariableType>();

            foreach (KeyValuePair<Identifier, Parser.AST.Type> kv in declaration.Fields)
            {
                fields.Add(kv.Key.Symbol, new VariableType(kv.Value, environment, kv.Value.Location));
            }

            Variable variable = environment.DeclareVariable(declaration.Name.Symbol, new StructValue(fields), new VariableSettings(new VariableType(Values.ValueType.Struct), declaration.Location), declaration.Location);
            return new VariableReference(variable);
        }
    }
}
