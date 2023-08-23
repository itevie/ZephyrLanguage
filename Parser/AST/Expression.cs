using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Lexer;

namespace Zephyr.Parser.AST
{
    internal class Expression
    {
        public Kind Kind { get; set; } = AST.Kind.Unknown;
        public Location? Location { get; set; } = null;
        public Location? FullExpressionLocation { get; set; } = null;
    }
}
