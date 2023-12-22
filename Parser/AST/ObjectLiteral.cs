using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class ObjectLiteral : Expression
    {
        public Dictionary<Identifier, Expression> KeyValuePairs = new Dictionary<Identifier, Expression>();

        public ObjectLiteral(Location location, Dictionary<Identifier, Expression> keyValuePairs)
            : base(Kind.ObjectLiteral, location)
        {
            KeyValuePairs = keyValuePairs;
        }
    }
}
