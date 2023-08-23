using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Helpers
    {
        public static NativeFunction GetTypeFunction(string type, string func, Environment env, Location? loc)
        {
            ObjectValue anyPkg = (ObjectValue)env.LookupVariable("Any");
            ObjectValue givenPkg = (ObjectValue)env.LookupVariable(type);

            NativeFunction givenFunc;

            if (givenPkg.Properties.ContainsKey(func) && ((NativeFunction)givenPkg.Properties[func]).Options.UsableAsTypeFunction)
            {
                givenFunc = (NativeFunction)givenPkg.Properties[func];
            } else if (anyPkg.Properties.ContainsKey(func) && ((NativeFunction)anyPkg.Properties[func]).Options.UsableAsTypeFunction)
            {
                givenFunc = (NativeFunction)anyPkg.Properties[func];
            } else
            {
                throw new RuntimeException(new()
                {
                    Location = loc,
                    Error = $"{type} type does not contain this"
                });
            }

            NativeFunction f = Values.Helpers.Helpers.CreateNativeFunction(givenFunc.Call);
            f.Name = givenFunc.Name;
            f.Options = givenFunc.Options;
            f.IsTypeCall = true;

            return f;
        }
    }
}
