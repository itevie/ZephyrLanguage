using ZephyrNew.Parser.AST;
using ZephyrNew.Runtime.Exceptions;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.Handlers
{
    internal partial class Statements
    {
        public static RuntimeValue ParseForEachStatement(ForEachStatement statement, Environment environment)
        {
            RuntimeValue right = Interpreter.Evaluate(statement.Right, environment);
            ArrayValue array = right.Enummerate();

            for (int i = 0; i != array.Items.Count; i++)
            {
                RuntimeValue value = array.Items[i];

                Environment loopEnv = new Environment(environment);
                loopEnv.DeclareVariable(
                    statement.Declaration.Identifier.Symbol,
                    value,
                    new VariableSettings(
                        new VariableType(
                            statement.Declaration.Type,
                            environment,
                            statement.Declaration.Type.Location
                        ),
                        statement.Declaration.Identifier.Location
                    ),
                    statement.Declaration.Identifier.Location
                );

                // Check if should declare index
                if (statement.DeclarationIndex != null)
                {
                    loopEnv.DeclareVariable(
                        statement.DeclarationIndex.Identifier.Symbol,
                        new NumberValue(i),
                        new VariableSettings(
                            new VariableType(
                                statement.DeclarationIndex.Type,
                                environment,
                                statement.DeclarationIndex.Type.Location
                            ),
                            statement.DeclarationIndex.Identifier.Location
                        ),
                        statement.DeclarationIndex.Identifier.Location
                    );
                }

                try
                {
                    Interpreter.Evaluate(statement.Body, loopEnv);
                }
                catch (BreakException _)
                {
                    break;
                }
                catch (ContinueException _)
                {
                    continue;
                }
            }

            return new NullValue();
        }
    }
}
