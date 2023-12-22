using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateCastExpression(CastExpression cast, Environment environment)
        {
            return Values.Helpers.CastValue(Interpreter.Evaluate(cast.Value, environment), new VariableType(cast.CastTo, environment, cast.CastTo.Location));
        }
    }
}
