using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Parser.AST
{
    internal class IndexerExpression : Expression
    {
        public Expression Left { get; set; } = new Expression();
        public Expression Indexer { get; set; } = new Expression();

        public IndexerExpression()
        {
            Kind = Kind.IndexerExpression;
        }
    }
}
