using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Values
{
    internal class FunctionValue : RuntimeValue
    {
        public string Name { get; set; } = "";
        public List<string> Parameters { get; set; } = new List<string>();
        public Environment DeclarationEnvironment { get; set; }
        public Expression Body { get; set; } = new Expression();

        public FunctionValue(Environment declarationEnvironment)
        {
            Type = ValueType.Function;
            DeclarationEnvironment = declarationEnvironment;
        }
    }
}
