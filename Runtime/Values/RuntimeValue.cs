using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser;

namespace ZephyrNew.Runtime.Values
{
    internal class RuntimeValue
    {
        public VariableType Type { get; set; } = new VariableType(ValueType.Any);
        public List<Modifier> Modifiers { get; set; } = new List<Modifier>();
        public Dictionary<string, RuntimeValue> Properties = new Dictionary<string, RuntimeValue>();
        public Location Location = Location.UnknownLocation;
        public bool CanHaveGenerics = false;

        // ----- Methods -----
        public void SetType(ValueType type)
        {
            Type = new VariableType(type);
        }

        // ----- Modifier Methods -----
        public bool HasModifier(Modifier modifier)
        {
            return Modifiers.Contains(modifier);
        }

        public void CheckFinal(Location? location = null)
        {
            if (Modifiers.Contains(Modifier.Final))
            {
                throw new RuntimeException($"Cannot modify a value with the final modifier", location ?? Location.UnknownLocation);
            }
        }

        /// <summary>
        /// Adds a modifier to the list of modifiers
        /// This is marked as virtual incase recursive modifier application is required
        /// </summary>
        /// <param name="modifier"></param>
        public virtual void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
        }

        // ----- Properties -----
        public RuntimeValue GetProperty(string property, Location? location = null)
        {
            if (Properties.ContainsKey(property) == false)
                return new NullValue();

            return Properties[property];
        }

        public RuntimeValue AddProperty(string property, RuntimeValue value, Location? location = null)
        {
            CheckFinal(location);
            Properties[property] = value;
            Helpers.TypeMatches(Type, this);
            if (Properties.ContainsKey(property))
                return new NullValue();
            return value;
        }

        public RuntimeValue SetProperty(string property, RuntimeValue value, Location? location = null)
        {
            CheckFinal(location);
            Properties[property] = value;
            Helpers.TypeMatches(Type, this);
            return value;
        }

        public bool HasProperty(string property)
        {
            return Properties.ContainsKey(property);
        }

        public virtual void UpdateValueProperties()
        {

        }

        public virtual void Keys()
        {
            throw new RuntimeException($"Cannot get keys of a {Helpers.VisualiseType(Type)}", Location.UnknownLocation);
        }

        // ----- Other Generic Methods -----

        /// <summary>
        /// Used for logging a value, specifically for the REPL.
        /// Values can override this function to define their own Visualise functions.
        /// </summary>
        /// <param name="alone"></param>
        /// <param name="noColor"></param>
        /// <returns></returns>
        public virtual string Visualise(bool alone = true, bool noColor = false)
        {
            string value = $"<{Helpers.VisualiseType(Type)}>";
            return noColor ? value : value.Pastel(ConsoleColor.Gray);
        }

        /// <summary>
        /// Returns the length of the current value.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeException"></exception>
        public virtual long Length(Location? location = null)
        {
            throw new RuntimeException($"Cannot get length of type {Helpers.VisualiseType(Type)}", location == null ? Location.UnknownLocation : location);
        }

        /// <summary>
        /// Returns an array containing a list of items. E.g., running this on a
        /// string will return a string array containing each character
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeException">Thrown when this function has not been overrwitten</exception>
        public virtual ArrayValue Enummerate(Location? location = null)
        {
            throw new RuntimeException($"{Helpers.VisualiseType(Type)} is not enummerable", location != null ? location : Location.UnknownLocation);
        }
    }
}
