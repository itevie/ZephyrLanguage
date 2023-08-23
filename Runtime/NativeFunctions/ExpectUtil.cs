using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class Util
    {
        public static void ExpectExact(List<RuntimeValue> args, List<Values.ValueType> expecting, Parser.AST.Expression? expression = null)
        {
            // Check amount
            if (args.Count != expecting.Count)
                throw new RuntimeException(new()
                {
                    Location = Handlers.Helpers.GetLocation(expression?.FullExpressionLocation, expression?.Location),
                    Error = $"Expected {expecting.Count} arguments but got {args.Count}"
                });

            for (int i = 0; i < expecting.Count; i++)
            {
                // Check if correct type
                if (args[i].Type != expecting[i] && expecting[i] != Values.ValueType.Any)
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(args[i].Location, expression?.FullExpressionLocation, expression?.Location),
                        Error = $"Expected argument {+i + 1} to be of type {expecting[i]} but got {args[i].Type}"
                    });
            }

            return;
        }
    }
}
