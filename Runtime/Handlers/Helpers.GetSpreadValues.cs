using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static List<RuntimeValue> GetSpreadValues(SpreadExpression spread, Environment environment)
        {
            RuntimeValue value = Interpreter.Evaluate(spread.Right, environment);

            if (value.Type.TypeName != Values.ValueType.Array)
            {
                throw new RuntimeException($"Expected array to spread", spread.Location);
            }

            return ((ArrayValue)value).Items;
        }
    }
}
