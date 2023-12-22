using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class FunctionParameter
    {
        public Location Location;
        public Identifier Name;
        public Type Type;
        public Expression? DefaultValue { get; set; } = null;
        public bool IsOptional { get; set; } = false;
        public bool IsParams { get; set; } = false;

        public FunctionParameter(Location location, Identifier name, Type type)
        {
            Location = location;
            Name = name;
            Type = type;
        }
    }
}
