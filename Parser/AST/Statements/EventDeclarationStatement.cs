using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class EventDeclarationStatement : Expression
    {
        public TypeExpression Type { get; set; } = new TypeExpression();
        public Identifier Identifier { get; set; } = new Identifier();

        public EventDeclarationStatement()
        {
            Kind = Kind.EventDeclaration;
        }
    }
}
