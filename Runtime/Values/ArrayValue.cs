using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime.Values
{
    internal class ArrayValue : RuntimeValue
    {
        public List<RuntimeValue> Items = new List<RuntimeValue>();

        public ArrayValue(List<RuntimeValue> items, VariableType completeType)
        {
            Items = items;
            SetType(ValueType.Array);
            Type = completeType;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string finished = $"<Array {Values.Helpers.VisualiseType(Type)}: ";

            foreach (RuntimeValue value in Items)
            {
                finished += value.Visualise(false) + ", ";
            }

            finished = finished.Remove(finished.Length - 2) + $">";

            return noColor ? finished : finished.Pastel(ConsoleColor.Gray);
        }

        public override long Length(Location? location = null)
        {
            return Items.Count;
        }

        public override ArrayValue Enummerate(Location? location = null)
        {
            return this;
        }
    }
}
