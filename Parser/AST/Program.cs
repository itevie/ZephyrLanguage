using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class Program : Expression
    {
        public List<Expression> Body = new List<Expression>();

        public Program(Location location) : base(Kind.Program, location)
        {

        }
    }
}
