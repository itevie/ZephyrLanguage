using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;

namespace ZephyrNew.Parser.AST
{
    internal class FunctionDeclaration : Expression
    {
        public Identifier Name;
        public Type ReturnType;
        public Expression Body;
        public List<FunctionParameter> Parameters = new List<FunctionParameter>();

        public FunctionDeclaration(Location location, Identifier name, Type returnType, List<FunctionParameter> parameters, Expression body)
            : base(Kind.FunctionDeclaration, location)
        {
            NeedsSemicolon = false;
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }
    }
}
