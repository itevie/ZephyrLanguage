using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class ModuleValue : RuntimeValue
    {
        public Dictionary<string, RuntimeValue> Variables;
        public string Origin;

        public ModuleValue(Dictionary<string, RuntimeValue> variables, string origin)
        {
            Variables = variables;
            Origin = origin;
            Type = new VariableType(Values.ValueType.Module);
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string value = ""; 

            if (noColor)
                value += $"<Module {Origin}> ";
            else value += $"<Module {Origin}> ".Pastel(ConsoleColor.Gray);

            value += Helpers.VisualiseObject(Variables);

            return value;
        }
    }
}
