using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class VariableReference : RuntimeValue
    {
        public Variable Variable;

        public VariableReference(Variable variable)
        {
            SetType(ValueType.VariableReference);
            Variable = variable;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string val = $"<VariableReference {Helpers.VisualiseType(Variable.Settings.Type)} {Variable.Settings.Name}: {Variable.Value.Visualise()}>";
            return noColor == true ? val : val.Pastel(ConsoleColor.Gray);
        }
    }
}
