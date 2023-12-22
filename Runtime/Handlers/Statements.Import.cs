using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateImportStatement(ImportStatement statement, Environment environment)
        {
            // Try to read the file
            string pathName = ZephyrPath.Resolve(environment.Directory, statement.From.Value);
            Debug.Log($"Attempting to import {pathName}", LogType.Modules);
            Environment importEnv = new Environment(new FileInfo(pathName).Directory.FullName);

            if (!ImportedModules.HasModule(pathName))
            {
                Debug.Log($"Not already imported previously: {pathName}", LogType.Modules);
                if (!File.Exists(pathName))
                {
                    throw new RuntimeException($"Cannot find the file {Path.GetFullPath(pathName)} to import", statement.From.Location);
                }

                // Read it
                string contents = File.ReadAllText(pathName);

                // Run it
                Parser.AST.Program program = new Parser.Parser().ProduceAST(contents, pathName);
                Interpreter.Evaluate(program, importEnv);
                ImportedModules.AddModule(pathName, importEnv);
            } else
            {
                Debug.Log($"No need to import as it has been previously imported: {pathName}", LogType.Modules);
                importEnv = ImportedModules.GetModule(pathName);
            }

            // Collect public ones
            Dictionary<string, RuntimeValue> publicVariables = importEnv.GetPublicVariables();

            // Craete variable
            ModuleValue moduleValue = new ModuleValue(publicVariables, pathName);
            ObjectValue toReturn = new ObjectValue();

            // Declare - import x as x
            if (statement.ImportAs != null)
            {
                environment.DeclareVariable(statement.ImportAs.Symbol, moduleValue, new VariableSettings(new VariableType(Values.ValueType.Module), statement.Location), statement.Location);
                /*ObjectValue temp = new ObjectValue();
                foreach (KeyValuePair<string, RuntimeValue> kv in moduleValue.Variables)
                {
                    temp.AddProperty(kv.Key, kv.Value);
                }*/
                toReturn.AddProperty(statement.ImportAs.Symbol, moduleValue);
            }

            // Declare - from x import x
            else
            {
                foreach (Identifier ident in statement.ImportIdentifiers)
                {
                    // Check if it declares it
                    if (publicVariables.ContainsKey(ident.Symbol) == false)
                        throw new RuntimeException($"The module {pathName} does not declare a public value: {ident.Symbol}", ident.Location);
                    environment.DeclareVariable(
                        ident.Symbol,
                        publicVariables[ident.Symbol],
                        new VariableSettings(publicVariables[ident.Symbol].Type, ident.Location), ident.Location);
                    toReturn.AddProperty(ident.Symbol, publicVariables[ident.Symbol]);
                }
            }

            return toReturn;
        }
    }
}
