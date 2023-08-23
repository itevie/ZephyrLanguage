using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Runner
{
    internal class RunnerException : Exception
    {
        public RunnerException(string err) : base(err)
        {

        }
    }
}
