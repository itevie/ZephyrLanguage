using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateVarref(VarrefExpression expression, Environment environment)
        {
            Variable variable = environment.LookupVariableReturnVariable(expression.Identifier.Symbol, expression.Identifier);
            return new Values.VariableValue(variable);
        }
    }
}
