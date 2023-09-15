using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zephyr.Parser;
using Zephyr.Parser.AST;

namespace Zephyr.Runtime.Values
{
    internal class EventValue : RuntimeValue
    {
        public List<FunctionValue> Listeners = new();
        public ValueType EventType = ValueType.Any;
        public string EventName = "UnknownEvent";

        public void AddListener(FunctionValue func)
        {
            if (Listeners.Contains(func))
            {
                throw new RuntimeException(new()
                {
                    Error = "Cannot add listener because this function is already listening for this event"
                });
            }

            Listeners.Add(func);
        }

        public void RemoveListener(FunctionValue func)
        {
            if (!Listeners.Contains(func))
            {
                throw new RuntimeException(new()
                {
                    Error = "Cannot remove listener because this function is not listening to this event"
                });
            }

            Listeners.Remove(func);
        }

        public void ExecuteListeners(RuntimeValue value, Environment environment)
        {
            foreach (FunctionValue function in Listeners)
            {
                Handlers.Helpers.EvaluateFunctionHelper(function, new() { value }, environment);
            }
        }

        public EventValue()
        {
            Type = ValueType.Event;
        }
    }
}
