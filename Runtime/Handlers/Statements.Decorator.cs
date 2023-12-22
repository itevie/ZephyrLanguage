using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue EvaluateDecoratorStatement(DecoratorApplierStatement decorator, Environment environment)
        {
            // Gather function name
            string functionName;
            if (decorator.Applyee.Kind == Kind.Identifier)
                functionName = ((Identifier)decorator.Applyee).Symbol;
            else throw new RuntimeException($"", decorator.Location);

            // Get the function
            RuntimeValue functionValuePre = environment.LookupVariable(functionName, decorator.Location);

            // Expect function
            if (functionValuePre.Type.TypeName != Values.ValueType.Function)
                throw new RuntimeException($"Expected function", decorator.Applyee.Location);
            FunctionValue functionValue = (FunctionValue)functionValuePre;

            // Check return type
            if (functionValue.ReturnType.TypeName != Values.ValueType.Function)
                throw new RuntimeException($"Expected decorator function to return a function", decorator.Applyee.Location);

            // Get the body
            RuntimeValue body = Interpreter.Evaluate(decorator.Body, environment);

            // Expect var ref
            if (body.Type.TypeName != Values.ValueType.VariableReference)
                throw new RuntimeException($"Expected variable reference", decorator.Body.Location);
            if (((VariableReference)body).Variable.Value.Type.TypeName != Values.ValueType.Function)
                throw new RuntimeException($"The variable reference does not refer to a function value", decorator.Body.Location);

            VariableReference varRef = ((VariableReference)body);
            FunctionValue func = ((FunctionValue)varRef.Variable.Value);

            // Apply decorator
            func.Decorators.Add(functionValue);

            // Done
            return varRef;
        }
    }
}
