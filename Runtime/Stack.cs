using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Lexer;
using ZephyrNew.Runtime.Values;

namespace ZephyrNew.Runtime
{
    internal static class StackContainer
    {
        public static Dictionary<int, Stack> Stacks = new Dictionary<int, Stack>();

        public static Stack GetStack()
        {
            if (!Stacks.ContainsKey(System.Environment.CurrentManagedThreadId))
                return new Stack();
            return Stacks[System.Environment.CurrentManagedThreadId];
        }

        public static Stack CreateStack(Stack? parent = null)
        {
            lock (Stacks)
            {
                Debug.Log($"Stack created: {System.Environment.CurrentManagedThreadId}", LogType.Stack);
                Stacks[System.Environment.CurrentManagedThreadId] = new Stack(parent);
                return Stacks[System.Environment.CurrentManagedThreadId];
            }
        }

        public static void PushToStack(StackItem item)
        {
            lock (Stacks)
            {
                Stack stack = GetStack();
                Debug.Log($"Pushed to stack {stack.Id}: {item.Value.Visualise()}", LogType.Stack);
                stack.AddItem(item);

                if (stack.Stacktrace.Count > 100)
                {
                    throw new RuntimeException($"Stack overflow", item.Value.DeclaredAt);
                }
            }
        }

        public static void PopFromStack()
        {
            lock (Stacks)
            {
                Stack stack = GetStack();
                Debug.Log($"Popped from stack {stack.Id}", LogType.Stack);
                if (stack.Stacktrace.Count > 0)
                    stack.Stacktrace.RemoveAt(stack.Stacktrace.Count - 1);
            }
        }

        // ----- Dangerous -----
        public static void ClearStacks()
        {
            Stacks = new Dictionary<int, Stack>();
        }
    }

    internal class Stack
    {
        public Stack? Parent = null;
        public int Id;
        public List<StackItem> Stacktrace = new List<StackItem>();

        public Stack(Stack? parent = null)
        {
            Id = System.Environment.CurrentManagedThreadId;
            Parent = parent;

            if (parent != null)
            {
                // Clone parent into this one
                foreach (StackItem item in parent.Stacktrace)
                {
                    Stacktrace.Add(item);
                }
            }
        }

        public void AddItem(StackItem item)
        {
            Stacktrace.Add(item);
        }

        public string Visualise()
        {
            string result = "";

            List<StackItem> stacktrace = Stacktrace.ToArray().ToList();
            stacktrace.Reverse();

            foreach (StackItem item in stacktrace)
            {
                string current = $" at {item.Value.VisualiseCompact()}:{item.Value.DeclaredAt.GenerateSimple()}\n";
                result += current;
            }

            return result;
        }
    }

    internal class StackItem
    {
        public FunctionValue Value;
        public Location Location;

        public StackItem(FunctionValue functionValue, Location location)
        {
            Value = functionValue;
            Location = location;
        }
    }
}
