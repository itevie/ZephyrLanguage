using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Runtime.Values;

namespace Zephyr.Runtime.NativeFunctions
{
    internal partial class NativeFunctions
    {
        public static Package ModifiersPkg = new("Modifiers", new
        {
            _final = (int)Modifier.Final
        });
    }
}
