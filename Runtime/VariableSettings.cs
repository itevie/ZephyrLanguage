using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime
{
    internal class VariableSettings
    {
        public bool IsConstant { get; set; } = false;
        public Values.ValueType Type { get; set; } = Values.ValueType.Any;
        public bool IsNullable { get; set; } = false;
        public string? Origin { get; set; } = null;
        public List<Values.Modifier> Modifiers { get; set; } = new();
        public Expression? DeclaredAt { get; set; } = null;
    }
}
