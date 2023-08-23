using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;
using Zephyr.Parser;
using Zephyr.Parser.AST;
using Zephyr.Runtime.NativeFunctions;
using Zephyr.Runtime.Values;
using Zephyr.Runtime.Values.Helpers;

namespace Zephyr.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateImportStatement(ImportStatement statement, Environment environment)
        {
            string toImport = ((StringLiteral)statement.ToImport).Value;
            bool isNative = toImport.StartsWith("@");

            // Check for package
            if (toImport.StartsWith("./") == false && !isNative)
            {
                // Check packages
                if (Program.LoadedPackages.ContainsKey(toImport))
                {
                    toImport = Program.LoadedPackages[toImport].EntryPoint;
                } else
                {
                    throw new RuntimeException(new()
                    {
                        Location = statement.ToImport.Location,
                        Error = $"Cannot find the package {toImport}"
                    });
                }
            } else 
            {
                toImport = Path.Combine(environment.Directory, toImport.Replace("./", ""));
            }

            Debug.Log($"Loading file, entry point = {toImport}");

            if (isNative)
            {
                // Check if it exists
                NonDefaultPackage? package = null;

                FieldInfo[] nativeFunctionProperties = typeof(NativeFunctions.Packages).GetFields(BindingFlags.Public | BindingFlags.Static);

                // Load all properties from Packages
                foreach (FieldInfo fieldInfo in nativeFunctionProperties)
                {
                    // Get the package value
                    NonDefaultPackage? pkg = (NonDefaultPackage?)fieldInfo.GetValue(fieldInfo?.DeclaringType);
                    if (pkg != null && pkg.Name == toImport.Replace("@", ""))
                    {
                        package = pkg;
                    }
                }

                // Check if it was found
                if (package == null)
                {
                    throw new RuntimeException(new()
                    {
                        Location = statement.ToImport.Location,
                        Error = $"Cannot find the package {toImport}"
                    });
                }

                // Init
                string? error = package.Validate();

                if (error != null)
                {
                    throw new RuntimeException(new()
                    {
                        Location = statement.ToImport.Location,
                        Error = error,
                    });
                }

                // Declare it
                environment.DeclareVariable(statement.ImportAs != null ? ((Identifier)statement.ImportAs).Symbol : package.Name, Values.Helpers.Helpers.CreateObject(package.Object), new VariableSettings()
                {
                    IsConstant = true,
                });
            } else
            {
                // Try read it
                if (File.Exists(toImport))
                {
                    string declareAs = "";

                    if (statement.ImportAs == null)
                    {
                        throw new RuntimeException(new()
                        {
                            Location = statement.ToImport.Location,
                            Error = $"Need to use as keyword when importing Zephyr file"
                        });
                    }

                    declareAs = ((Identifier)statement.ImportAs).Symbol;

                    Environment fileScope = new Environment(Runner.FileExecutor.GlobalEnvironment);
                    Debug.Log($"Importing with directory {new FileInfo(toImport).Directory.FullName} ({toImport})");
                    fileScope.Directory = new FileInfo(toImport).Directory.FullName;

                    Parser.AST.Program program = new Parser.Parser().ProduceAST(File.ReadAllText(toImport), toImport);
                    Interpreter.Evaluate(program, fileScope);

                    ObjectValue obj = Values.Helpers.Helpers.CreateObject(new { });

                    // Define in current scope
                    foreach (var val in fileScope.ExportedVariables)
                    {
                        obj.Properties.Add(val.Key, val.Value);
                    }

                    environment.DeclareVariable(declareAs, obj, new VariableSettings()
                    {
                        IsConstant = true
                    }, statement.ToImport);
                } else
                {
                    throw new RuntimeException(new()
                    {
                        Location = statement.ToImport.Location,
                        Error = $"Cannot find this package"
                    });
                }
            }

            return Values.Helpers.Helpers.CreateNull();
        }
    }
}
