﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class StringValue : RuntimeValue
    {
        public string Value = "";

        public StringValue()
        {
            Type = Values.ValueType.String;
        }
    }
}