using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Literals
    {
        public static RuntimeValue ParseObjectLiteral(ObjectLiteral literal, Environment environment)
        {
            ObjectValue value = new ObjectValue();
            value.Location = literal.Location;

            foreach (KeyValuePair<Identifier, Expression> keyValuePair in literal.KeyValuePairs) {
                RuntimeValue v = Interpreter.Evaluate(keyValuePair.Value, environment);

                value.SetProperty(keyValuePair.Key.Symbol, v, keyValuePair.Key.Location);
            }

            return value;
        }
    }
}
