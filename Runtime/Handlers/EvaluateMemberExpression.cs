using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.NativeFunctions;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Expressions
    {
        public static RuntimeValue EvaluateMemberExpression(Parser.AST.MemberExpression expression, Environment environment)
        {
            RuntimeValue obj = Interpreter.Evaluate(expression.Object, environment);

            // Check if it is a type that can have functions on it
            string? type = null;
            switch (obj.Type)
            {
                case Values.ValueType.Int:
                    type = "Integer";
                    break;
                case Values.ValueType.Array:
                    type = "Array";
                    break;
                case Values.ValueType.String:
                    type = "String";
                    break;
                case Values.ValueType.Null:
                    type = "Any";
                    break;
                case Values.ValueType.Object:
                    type = "Object";
                    break;
                case Values.ValueType.Float:
                    type = "Float";
                    break;
            }

            // If it is an object do something special
            if (type == "Object")
            {
                try
                {
                    NativeFunction func = Helpers.GetTypeFunction(type, ((Identifier)expression.Property).Symbol, environment, Helpers.GetLocation(obj.Location, Helpers.GetLocation(obj.Location, expression.Location)));
                    func.IsTypeCall = true;
                    func.TypeCallValue = obj;
                    return func;
                } catch
                {
                    type = null;
                }
            }

            if (type != null)
            {
                NativeFunction func = Helpers.GetTypeFunction(type, ((Identifier)expression.Property).Symbol, environment, Helpers.GetLocation(obj.Location, Helpers.GetLocation(obj.Location, expression.Location)));
                func.IsTypeCall = true;
                func.TypeCallValue = obj;
                return func;
            }

            // Check if it is an object
            if (obj.Type != Values.ValueType.Object && obj.Type != Values.ValueType.Array)
            {
                throw new Exception($"Cannot access a {obj.Type} via member expression");
            }

            // Check if indexing array
            if (obj.Type == Values.ValueType.Array)
            {
                Location? location = Helpers.GetLocation(expression.FullExpressionLocation, expression.Property.FullExpressionLocation, expression.Property.Location);
                if (expression.IsComputed == false)
                {
                    throw new RuntimeException(new()
                    {
                        Location = expression.Location,
                        Error = $"Expected indexer but got {expression.Property.Kind}"
                    });
                }

                RuntimeValue index = Interpreter.Evaluate(expression.Property, environment);

                // Check type
                if (index.Type != Values.ValueType.Number)
                {
                    throw new RuntimeException(new()
                    {
                        Location = location,
                        Error = $"Expected a number to index an array, but got {index.Type}"
                    });
                }

                int idx = (int)((IntegerValue)index).Value;

                ArrayValue val = (ArrayValue)obj;

                // Check if it contains index
                if (idx < 0 || val.Items.Count - 1 < idx)
                {
                    throw new RuntimeException(new()
                    {
                        Location = location,
                        Error = $"Index out of bounds"
                    });
                }

                return val.Items[idx];
            }

            string propertyName;

            // Check dot chain
            if (expression.IsComputed)
            {
                RuntimeValue val = Interpreter.Evaluate(expression.Property, environment);

                // Check if ident
                if (val.Type != Values.ValueType.String)
                    throw new RuntimeException(new()
                    {
                        Location = expression.Property.Location,
                        Error = $"Cannot use {val.Type} as an indexer"
                    });
                propertyName = ((StringValue)val).Value;
            }
            else
            {
                propertyName = ((Parser.AST.Identifier)expression.Property).Symbol;
            }

            // Check if object has the property
            if (((ObjectValue)obj).Properties.ContainsKey(propertyName) == false)
            {
                throw new RuntimeException(new()
                {
                    Location = expression.Property.Location,
                    Error = $"Object does not contain a definition for {propertyName}"
                });
            }

            // Get the property
            RuntimeValue property = ((ObjectValue)obj).Properties[propertyName];

            return property;
        }
    }
}
