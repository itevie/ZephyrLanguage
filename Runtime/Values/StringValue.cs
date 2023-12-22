using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Runtime.Values
{
    internal class StringValue : RuntimeValue
    {
        private string _value;

        public string Value { get { return _value; } set { _value = value; } }

        public StringValue(string value)
        {
            SetType(ValueType.String);
            _value = value;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string text = $"{(alone ? "" : "\"")}{Value}{(alone ? "" : "\"")}";
            return alone ? text : text.Pastel(ConsoleColor.Magenta);
        }

        public override long Length(Location? location = null)
        {
            return Value.Length;
        }

        public override ArrayValue Enummerate(Location? location = null)
        {
            return new ArrayValue(Value.ToCharArray().Select(x => (RuntimeValue)new StringValue(x.ToString())).ToList(), new VariableType(ValueType.Array)
            {
                ArrayDepth = 1,
                IsArray = true,
                ArrayType = ValueType.String,
            });
        }
    }
}
