using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;

namespace ZephyrNew.Runtime
{
    internal class VariableSettings
    {
        public VariableType Type { get; set; }
        public Location DeclaredAt { get; set; }
        public bool ForceDefinition { get; set; } = false;
        public string Name { get; set; } = "<Unknown>";

        public VariableSettings(VariableType type, Location declaredAt)
        {
            Type = type;
            DeclaredAt = declaredAt;
        }
    }
}
