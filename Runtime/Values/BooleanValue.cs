using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class BooleanValue : RuntimeValue
    {
        public bool Value { get; set; }

        public BooleanValue(bool value)
        {
            SetType(ValueType.Boolean);
            Value = value;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string text = $"{(Value ? "true" : "false")}";

            if (!noColor)
            {
                if (Value)
                    text = text.Pastel(ConsoleColor.Green);
                else text = text.Pastel(ConsoleColor.Red);
            }

            return text;
        }
    }
}
