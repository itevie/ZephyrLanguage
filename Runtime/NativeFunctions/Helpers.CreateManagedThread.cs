using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Helpers
    {
        public static void CreateManagedThread(Action action, Stack stack)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                StackContainer.CreateStack(stack);
                try
                {
                    action();
                } catch (RuntimeException ex)
                {
                    Console.WriteLine(ex.Visualise());
                }
            }));

            thread.Start();
        }
    }
}
