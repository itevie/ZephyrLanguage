using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class LambdaArgumentList : Expression
    {
        public List<Identifier> Arguments;

        public LambdaArgumentList(Location location, List<Identifier> arguments)
            : base(Kind.LambdaArgumentList, location)
        {
            Arguments = arguments;
        }
    }
}
