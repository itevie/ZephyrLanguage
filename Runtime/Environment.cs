using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.NativeFunctions;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime
{
    internal class Environment
    {
        private Environment? _parent;
        public Dictionary<string, Variable> _variables = new();
        public Dictionary<string, RuntimeValue> ExportedVariables = new();
        public string Directory = "";

        public Environment(Environment? parent = null)
        {
            _parent = parent;
            if (parent == null)
            {
                LoadGlobalVariables();
            }
        }

        public Values.RuntimeValue DeclareVariable(string variableName, RuntimeValue value, VariableSettings settings, Parser.AST.Expression? from = null)
        {
            // Check if variable already exists
            if (_variables.ContainsKey(variableName))
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = $"Cannot declare variable {variableName} because it already exists"
                });

            bool exists = false;

            if (settings.Modifiers.Count != 0)
            {
                value.Modifiers = settings.Modifiers;
            }

            // Recursively add modifiers
            AddModifiers(value, value.Modifiers);

            try
            {
                LookupVariable(variableName);
                exists = true;
            } catch { }
            
            if (exists == true && variableName.StartsWith("~") == false)
            {
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = $"Cannot declare variable {variableName} because it already exists"
                });
            }

            CheckType(value, settings, from);

            // Set it
            _variables[variableName] = new Variable(value, settings);
            Verbose.Log($"Declared variable {variableName}", $"environment {from?.Location?.FileName}");
            return value;
        }

        private RuntimeValue AddModifiers(RuntimeValue value, List<Modifier> modifiers)
        {
            switch (value.Type)
            {
                case Values.ValueType.Null:
                case Values.ValueType.Int:
                case Values.ValueType.Long:
                case Values.ValueType.Float:
                case Values.ValueType.Double:
                case Values.ValueType.String:
                case Values.ValueType.Boolean:
                case Values.ValueType.Function:
                case Values.ValueType.NativeFunction:
                case Values.ValueType.Enumerable:
                    value.Modifiers = modifiers;
                    return value;
                case Values.ValueType.Object:
                    value.Modifiers = modifiers;

                    foreach (KeyValuePair<string, RuntimeValue> val in ((ObjectValue)value).Properties)
                    {
                        AddModifiers(val.Value, modifiers);
                    }
                    return value;
                default:
                    value.Modifiers = modifiers;
                    return value;
            }
        }

        public Values.RuntimeValue AssignVariable(string variableName, RuntimeValue value, bool force = false, Expression? from = null)
        {
            Environment environment = Resolve(variableName);

            // Get the variable
            Variable variable = LookupVariable(variableName, true);

            // Check if it is constant
            if (variable.Options.IsConstant && force == false)
            {
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = "Cannot assign to a constant variable"
                });
            }

            CheckType(value, variable.Options, from);

            // Check for number cast
            if (Helpers.IsNumberValue(value.Type))
            {
                value = Handlers.Helpers.CastValueHelper(value, variable.Options.Type);
            }

            // Assign
            environment._variables[variableName].Value = value;

            return value;
        }

        public RuntimeValue LookupVariable(string variableName, Expression? from = null)
        {
            Environment environment = Resolve(variableName, from);
            return environment._variables[variableName].Value;
        }

        public Variable LookupVariable(string variableName, bool abc, Expression? from = null)
        {
            Environment environment = Resolve(variableName, from);
            return environment._variables[variableName];
        }

        public Environment Resolve(string variableName, Expression? from = null)
        {
            // Check if this env contains it
            if (_variables.ContainsKey(variableName))
                return this;

            // Check if there is a parent
            if (_parent == null)
                throw new RuntimeException(new()
                {
                    Location = from?.Location,
                    Error = $"Cannot find variable: {variableName}"
                });

            // Try resolve in parent
            return _parent.Resolve(variableName, from);
        }

        private void LoadGlobalVariables()
        {
            Debug.Log($"Loading global variables", $"global environment");

            VariableSettings defaultSettings = new()
            {
                IsConstant = true,
                Modifiers = new()
                {
                    Modifier.Final
                }
            };

            FieldInfo[] nativeFunctionProperties = typeof(NativeFunctions.NativeFunctions).GetFields(BindingFlags.Public | BindingFlags.Static);
            // Load all properties from NativeFunctions
            foreach (FieldInfo fieldInfo in nativeFunctionProperties)
            {
                // Get the package value
                Package? package = (Package?)fieldInfo.GetValue(fieldInfo?.DeclaringType);
                if (package != null)
                {
                    // Load it
                    RuntimeValue val = Values.Helpers.Helpers.CreateObject(package.Object);
                    val.Modifiers = new()
                    {
                        Modifier.Final
                    };
                    DeclareVariable(package.Name, val, defaultSettings);
                }
            }

            // Declare global functions
            DeclareVariable("typeof", Values.Helpers.Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.Any });
                return Values.Helpers.Helpers.CreateString(args[0].Type.ToString());
            }, options: new()
            {
                Name = "typeof",
                Parameters =
                {
                    new()
                    {
                        Name = "variable",
                        Type = Values.ValueType.Any
                    }
                }
            }), defaultSettings);

            // Declare default variables
            DeclareVariable("null", Values.Helpers.Helpers.CreateNull(), defaultSettings);
            DeclareVariable("true", Values.Helpers.Helpers.CreateBoolean(true), defaultSettings);
            DeclareVariable("false", Values.Helpers.Helpers.CreateBoolean(false), defaultSettings);

            DeclareVariable("eval", Helpers.CreateNativeFunction((args, env, expr) =>
            {
                Util.ExpectExact(args, new() { Values.ValueType.String });

                try
                {
                    // Generate env
                    Runtime.Environment environment = new(Runner.FileExecutor.GlobalEnvironment);
                    // Produce AST and execute
                    Parser.AST.Program program = new Parser.Parser().ProduceAST(((StringValue)args[0]).Value, "eval");
                    RuntimeValue value = Interpreter.Evaluate(program, env);
                    return value;
                } catch (ParserException e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                        Error = $"Parser error: {e.Message}"
                    });
                } catch (RuntimeException e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                        Error = $"Runtime error: {e.Message}"
                    });
                } catch (LexerException e)
                {
                    throw new RuntimeException(new()
                    {
                        Location = Handlers.Helpers.GetLocation(expr?.Location, args[0].Location),
                        Error = $"Lexer error: {e.Message}"
                    });
                }
            }, options: new() { Name = "eval", Parameters = { new() { Name = "evaluationString", Type = Values.ValueType.String } } }), defaultSettings);
        }

        private static void CheckType(RuntimeValue value, VariableSettings settings, Expression? from)
        {
            if (settings.Type != Values.ValueType.Any)
            {
                if (value.Type != settings.Type && !(Helpers.IsNumberValue(value.Type) && Helpers.IsNumberValue(settings.Type)))
                {
                    // Check null
                    if (value.Type == Values.ValueType.Null && settings.IsNullable == false)
                        throw new RuntimeException(new()
                        {
                            Location = from?.Location,
                            Error = "Cannot assign Null to a non-nullable variable"
                        });
                    else if (value.Type != Values.ValueType.Null)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = from?.Location,
                            Error = $"Cannot assign type {value.Type} to a variable of type {settings.Type}"
                        });
                    }
                }
            }
        }

        private static RuntimeValue ResolveType(RuntimeValue value, VariableSettings settings, Expression? from)
        {
            // Check if value is a number
            if (Helpers.IsNumberValue(value.Type))
            {
                // Check floats
                if (value.Type == Values.ValueType.Float && settings.Type != Values.ValueType.Float)
                {
                    throw new RuntimeException(new()
                    {
                        Location = from?.Location,
                        Error = $"Cannot assign float to a {settings.Type} variable"
                    });
                }

                // Just convert it
                return Helpers.CastNonFloatNumberValues(value, settings.Type);
            }

            return value;
        }
    }
}
