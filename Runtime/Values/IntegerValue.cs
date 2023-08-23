﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values
{
    internal class IntegerValue : RuntimeValue
    {
        public int Value = 0;

        public IntegerValue()
        {
            Type = ValueType.Int;
        }
    }
}
