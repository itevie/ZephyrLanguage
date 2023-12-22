using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class CallExpression : Expression
    {
        public Expression Callee;
        public List<Expression> Parameters;

        public CallExpression(Location location, Expression callee, List<Expression> parameters)
            : base(Kind.CallExpression, location)
        {
            Callee = callee;
            Parameters = parameters;
        }
    }
}
