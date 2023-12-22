using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Parser.AST;

namespace ZephyrNew.Runtime.Values
{
    internal class FunctionValue : RuntimeValue
    {
        public string Name { get; set; } = "";
        public List<FunctionParameter> Parameters { get; set; } = new List<FunctionParameter>();
        public Environment Environment;
        public Expression Body;
        public VariableType ReturnType;
        public Location DeclaredAt;
        public List<FunctionValue> Decorators = new List<FunctionValue>();

        public FunctionValue(Environment environment, Expression body, VariableType returnType, Location location)
        {
            CanHaveGenerics = true;
            SetType(Values.ValueType.Function);
            Environment = environment;
            Body = body;
            DeclaredAt = location;
            ReturnType = returnType;
        }

        public override string Visualise(bool alone = true, bool noColor = false)
        {
            string text = $"<function {Helpers.VisualiseType(ReturnType)} {Name}(";

            for (int i = 0; i != Parameters.Count; i++)
            {
                text += $"{Helpers.VisualiseType(Parameters[i].Type)} {Parameters[i].Name.Symbol}";

                if (i != Parameters.Count - 1)
                    text += ", ";
            }
                
            text += ")>";

            if (noColor) return text;
            return text.Pastel(ConsoleColor.Gray);
        }

        public string VisualiseCompact()
        {
            string text = $"{Helpers.VisualiseType(ReturnType)} {Name}(";

            for (int i = 0; i != Parameters.Count; i++)
            {
                text += $"{Helpers.VisualiseType(Parameters[i].Type)} {Parameters[i].Name.Symbol}";

                if (i != Parameters.Count - 1)
                    text += ", ";
            }
            text += ")";

            return text.Pastel(ConsoleColor.Gray);
        }
    }
}
