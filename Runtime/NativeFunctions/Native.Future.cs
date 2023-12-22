using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package FuturePkg = new Package("Future", new Values.ObjectValue(new
        {
            create = new NativeFunction((baseOptions) =>
            {
                FunctionValue? thenFunc = null;
                FunctionValue? catchFunc = null;
                ObjectValue returned = new ObjectValue();

                RuntimeValue? thenResult = null;
                RuntimeValue? catchResult = null;

                NativeFunction then = new NativeFunction((options) =>
                {
                    thenFunc = ((FunctionValue)options.Arguments[0]);

                    // Check if already completed
                    if (thenResult != null)
                        Handlers.Helpers.ExecuteZephyrFunction(thenFunc, new List<RuntimeValue>
                        {
                            thenResult
                        }, baseOptions.Environment);
                    return returned;
                }, new Options("then", new List<Parameter>()
                {
                    new Parameter(new VariableType(Values.ValueType.Function), "func"),
                }));

                NativeFunction catchF = new NativeFunction((options) =>
                {
                    catchFunc = ((FunctionValue)options.Arguments[0]);

                    // Check if already completed
                    if (catchResult != null)
                        Handlers.Helpers.ExecuteZephyrFunction(catchFunc, new List<RuntimeValue>
                        {
                            catchResult
                        }, baseOptions.Environment);

                    return returned;
                }, new Options("catch", new List<Parameter>()
                {
                    new Parameter(new VariableType(Values.ValueType.Function), "func"),
                }));

                returned = new ObjectValue(new
                {
                    then,
                    error = catchF,
                });

                FunctionValue executor = ((FunctionValue)baseOptions.Arguments[0]);

                Helpers.CreateManagedThread(() =>
                {
                    Handlers.Helpers.ExecuteZephyrFunction(executor, new List<RuntimeValue>
                    {
                        new NativeFunction((options) =>
                        {
                            thenResult = options.Arguments[0];
                            if (thenFunc != null)
                            {
                                Handlers.Helpers.ExecuteZephyrFunction(thenFunc, new List<RuntimeValue>
                                {
                                    options.Arguments[0],
                                }, baseOptions.Environment);
                            }
                            return new NullValue();
                        }, new Options("resolve", new List<Parameter>
                        {
                            new Parameter(new VariableType(Values.ValueType.Any), "resolveValue")
                        })),
                        new NativeFunction((options) =>
                        {
                            catchResult = options.Arguments[0];

                            if (catchFunc != null)
                            {
                                Handlers.Helpers.ExecuteZephyrFunction(catchFunc, new List<RuntimeValue>
                                {
                                    options.Arguments[0],
                                }, baseOptions.Environment);
                            }

                            return new NullValue();
                        }, new Options("reject", new List<Parameter>
                        {
                            new Parameter(new VariableType(Values.ValueType.Any), "rejectValue")
                        }))
                    }, baseOptions.Environment);
                }, StackContainer.GetStack());

                return returned;
            }, new Options("create")
            {
                Parameters = new List<Parameter>
                {
                    new Parameter(new VariableType(Values.ValueType.Function), "func"),
                }
            })
        }));
    }
}
