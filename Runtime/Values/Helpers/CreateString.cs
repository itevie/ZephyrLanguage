﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runtime.Values.Helpers
{
    internal partial class Helpers
    {
        public static StringValue CreateString(string value)
        {
            return new StringValue()
            {
                Value = value,
            };
        }
    }
}