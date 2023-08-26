using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class ProjectCreator
    {
        public static void CreateNewProject()
        {
            string projectName = GetStringResponse("Project name: ");

            Console.WriteLine($"Setting up project {projectName}");

            // Check if directory already exists
            if (Directory.Exists(projectName))
            {
                ThrowError($"Cannot create project: Directory {projectName} already exists");
            }

            // Create directory
            string dir = Path.Combine(Directory.GetCurrentDirectory(), projectName);
            try
            {
                Console.WriteLine($"Creating directory: {dir}...");
                Directory.CreateDirectory(dir);
            } catch (Exception e)
            {
                ThrowError($"Cannot create project: Failed to create project directory: {e.Message}");
            }
            Console.WriteLine($"Setting up structure...");

            Directory.CreateDirectory(Path.Combine(dir, "src"));
            Directory.CreateDirectory(Path.Combine(dir, "zephyr_packages"));

            ZephyrPackage package = new();
            package.Name = projectName;
            package.Dependencies.Add("is_null", new()
            {
                Version = "1.0.0"
            });

            string json = JsonConvert.SerializeObject(package, Formatting.Indented);

            File.WriteAllText(Path.Combine(dir, "package.json"), json);
            File.Create(Path.Combine(dir, "index.zr"));

            Console.WriteLine($"Project created!");
        }

        public static void ThrowError(string error)
        {
            Console.WriteLine($"\n! Error:\n! {error}\n!".Pastel(ConsoleColor.Red));
            Environment.Exit(0);
        }

        public static string GetStringResponse(string prompt)
        {
            Console.Write(prompt);
            string text = Console.ReadLine() ?? "";

            // Check if quit
            if (text == "")
            {
                System.Environment.Exit(0);
            }

            return text;
        }
    }
}
