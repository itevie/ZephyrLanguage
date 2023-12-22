using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.NativeFunctions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime
{
    /// <summary>
    /// Represents a Zephyr environment or rather a "scope"
    /// </summary>
    internal class Environment
    {
        /// <summary>
        /// Where all the variables are stored for the current environment
        /// </summary>
        private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();
        private Dictionary<string, Export>? ExportedVariables = null;


        /// <summary>
        /// Represents the parent environment
        /// </summary>
        private Environment? _parent = null;

        public string Directory;

        public Environment(Environment? parent = null)
        {
            Debug.Log($"Environment created: {parent?.Directory}", LogType.Environment);
            _parent = parent;
            Directory = parent?.Directory ?? "";

            if (parent == null) SetupGlobal();
        }

        public Environment(string directory, Environment? parent = null)
        {
            Debug.Log($"Environment created: {directory}", LogType.Environment);
            _parent = parent;
            Directory = directory;
            ExportedVariables = new Dictionary<string, Export>();

            if (parent == null) SetupGlobal();
        }

        // ----- Methods to do with managing variables -----
        public Dictionary<string, Variable> GetVariables()
        {
            return _variables;
        }

        public Variable DeclareVariable(string variableName, RuntimeValue value, VariableSettings settings, Location location)
        {
            settings.Name = variableName;

            // Check if the variable already exists
            if (_variables.ContainsKey(variableName) && !settings.ForceDefinition)
                throw new RuntimeException($"The varaible {variableName} already exists", location);

            // Check type
            if (Values.Helpers.TypeMatches(settings, value) != null)
                throw new RuntimeException($"Cannot assign as {Values.Helpers.TypeMatches(settings, value)}", location);

            // Check if it is discard
            if (variableName == "_")
                return new Variable(value, settings);

            //value.Type = settings.Type;

            // Declare it
            Debug.Log($"Variable declared: {variableName} {value.Visualise(noColor: true)}", LogType.Environment);
            _variables[variableName] = new Variable(value, settings);
            return _variables[variableName];
        }

        public RuntimeValue LookupVariable(string variableName, Location from)
        {
            // Check if discard
            if (variableName == "_")
                throw new RuntimeException($"_ cannot be found as this is the discard identifier", from);

            // Check if variable exists
            if (!AnywhereHasVariable(variableName))
                throw new RuntimeException($"The variable {variableName} does not exist", from);
            return ResolveVariable(variableName).Value;
        }

        public RuntimeValue AssignVariable(string variableName, RuntimeValue newValue, Location from)
        {
            Variable variable = ResolveVariable(variableName);

            if (Values.Helpers.TypeMatches(variable.Settings, newValue) != null)
                throw new RuntimeException($"Cannot assign as {Values.Helpers.TypeMatches(variable.Settings, newValue)}", from);

            // Check if final
            if (variable.Value.HasModifier(Modifier.Final))
            {
                throw new RuntimeException($"Cannot assign to a variable with the final modifier", from);
            }

            variable.Value = newValue;

            return variable.Value;
        }

        public Variable ResolveVariable(string variableName)
        {
            // Check if current environment contains it
            if (HasVariable(variableName))
                return _variables[variableName];

            // Check if parent contains it
            if (_parent != null)
                return _parent.ResolveVariable(variableName);
                
            // Variable not found
            throw new RuntimeException($"Failed to lookup variable: {variableName} as it does not exist in any enclosing scope", Location.UnknownLocation);
        }

        public RuntimeValue ExportVariable(string identifier, RuntimeValue value, Location exportedAt)
        {
            // Check if can export here
            if (ExportedVariables == null)
            {
                throw new RuntimeException($"Cannot export here", exportedAt);
            }

            // Check if already exported
            if (ExportedVariables.ContainsKey(identifier))
                throw new RuntimeException($"A value with name {identifier} has already been exported", exportedAt);

            // Export
            ExportedVariables[identifier] = new Export(value, identifier, exportedAt);

            return value;
        }

        public bool AnywhereHasVariable(string variableName)
        {
            return _variables.ContainsKey(variableName) || (_parent?.AnywhereHasVariable(variableName) ?? false);
        }

        public bool HasVariable(string variableName)
        {
            return _variables.ContainsKey(variableName);
        }

        public Dictionary<string, RuntimeValue> GetPublicVariables()
        {
            Dictionary<string, RuntimeValue> publicVariables = new Dictionary<string, RuntimeValue>();

            foreach (KeyValuePair<string, Export> keyValuePair in ExportedVariables)
            {
                publicVariables.Add(keyValuePair.Key, keyValuePair.Value.Value);
            }

            return publicVariables;
        }

        // ----- Global Setup -----
        public void SetupGlobal()
        {
            FieldInfo[] nativeFunctionProperties = typeof(NativeFunctions.Native).GetFields(BindingFlags.Public | BindingFlags.Static);

            // Load all properties from NativeFunctions
            foreach (FieldInfo fieldInfo in nativeFunctionProperties)
            {
                // Get the package value
                Package? package = (Package?)fieldInfo.GetValue(fieldInfo?.DeclaringType);
                if (package != null)
                {
                    // Load it
                    RuntimeValue val = package.Object;
                    DeclareVariable(package.Name, val, new VariableSettings(new VariableType(Values.ValueType.Object), Location.UnknownLocation), Location.UnknownLocation);
                }
            }

            DeclareVariable("true", new BooleanValue(true), new(new VariableType(Values.ValueType.Boolean), Location.UnknownLocation), Location.UnknownLocation);
            DeclareVariable("false", new BooleanValue(false), new(new VariableType(Values.ValueType.Boolean), Location.UnknownLocation), Location.UnknownLocation);
            DeclareVariable("null", new NullValue(), new(new VariableType(Values.ValueType.Null) { IsNullable = true, }, Location.UnknownLocation), Location.UnknownLocation);
        }
    }
}
