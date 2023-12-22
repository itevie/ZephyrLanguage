using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;

namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static string VisualiseType(Parser.AST.Type type)
        {
            string finished = $"";

            finished += type.TypeName;

            if (type.GenericsList != null)
            {
                finished += "<";
                foreach (Parser.AST.Type t in type.GenericsList)
                {
                    finished += VisualiseType(t);
                    finished += ", ";
                }
                finished += ">";
            }

            if (type.IsNullable) finished += "?";

            if (type.IsArray && type.ArrayDepth > 0)
            {
                for (int i = 0; i != type.ArrayDepth; i++)
                    finished += "[]";
            }

            return finished;
        }

        public static string VisualiseType(VariableType type)
        {
            string finished = $"";

            finished += type.IsStruct ? "struct " : "";

            finished += type.IsArray ? type.ArrayType : type.TypeName;

            if (type.GenericsList != null)
            {
                finished += "<";
                foreach (VariableType t in type.GenericsList)
                {
                    finished += VisualiseType(t);
                    finished += ", ";
                }
                finished += ">";
            }

            if (type.IsNullable) finished += "?";

            if (type.IsArray && type.ArrayDepth > 0)
            {
                for (int i = 0; i != type.ArrayDepth; i++)
                    finished += "[]";
            }

            return finished;
        }
    }
}
