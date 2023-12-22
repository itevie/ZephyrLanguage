using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class NumberValue : RuntimeValue
    {
        public double Value;

        public NumberValue(double value)
        {
            SetType(ValueType.Number);
            Value = value;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string text = $"{Value}";
            return noColor ? text : text.Pastel(ConsoleColor.Magenta);
        }
    }
}
