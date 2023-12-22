using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.Values
{
    internal class NullValue : RuntimeValue
    {
        public object? Value = null;

        public NullValue()
        {
            SetType(ValueType.Null);
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string text = $"null";
            return noColor ? text : text.Pastel(ConsoleColor.Gray);
        }
    }
}
