using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZephyrNew.Runner
{
    internal class Watcher
    {
        private string _filePath;

        public Watcher(string filePath)
        {
            _filePath = filePath;
        }

        public async Task Start()
        {
            using var watcher = new FileSystemWatcher(Environment.CurrentDirectory);

            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Filter = _filePath;

            watcher.Changed += Act;
            watcher.EnableRaisingEvents = true;

            await Task.Delay(-1);
        }

        public void Act(object o, FileSystemEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"File changed, re-running...".Pastel(ConsoleColor.Gray));
            FileExecutor.ExecuteFile(_filePath);
        }
    }
}
