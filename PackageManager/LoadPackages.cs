using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.PackageManager
{
    internal class PackageLoader
    {
        public static void LoadPackages()
        {
            // Check for package.json
            if (File.Exists(Path.Join(Program.EntryPoint, Config.PackageManagerFileName)))
            {
                ZephyrPackage? package;
                try
                {
                    package = JsonConvert.DeserializeObject<ZephyrPackage>(File.ReadAllText(Path.Combine(Program.EntryPoint, Config.PackageManagerFileName)));
                } catch (Exception ex)
                {
                    throw new ZephyrException("Error", true)
                    {
                        Error = $"Failed to load " + Path.Combine(Program.EntryPoint, Config.PackageManagerFileName),
                        MoreDetails = ex
                    };
                }

                if (package != null)
                {
                    Debug.Log($"Loading package references...", "package-loader");

                    // Check for zephyr_packages folder
                    if (!Directory.Exists(Path.Combine(Program.EntryPoint, "zephyr_packages")))
                    {
                        Debug.Warning($"Cannot load packages as there is no zephyr_packages folder ({Path.Combine(Program.EntryPoint, "zephyr_packages")})", "package-loader");
                        return;
                    }

                    // Read all dependencies
                    foreach (KeyValuePair<string, ZephyrPackageDependency> dep in package.Dependencies)
                    {
                        Debug.Log($"Loading package {dep.Key}...", "package-loader");

                        ZephyrLoadedPackage? loadedPackage = null;

                        // Find package
                        foreach (string dir in Directory.EnumerateDirectories(Path.Combine(Program.EntryPoint, "zephyr_packages")))
                        {
                            // Try read package.json
                            if (File.Exists(Path.Combine(dir, Config.PackageManagerFileName)))
                            {
                                // Read it
                                ZephyrPackage? pkg = JsonConvert.DeserializeObject<ZephyrPackage>(File.ReadAllText(Path.Combine(dir, Config.PackageManagerFileName)));

                                if (pkg != null && pkg.Name == dep.Key)
                                {
                                    // Check for entry point
                                    if (pkg.EntryPoint != "")
                                    {
                                        // Check if entry point exists
                                        if (File.Exists(Path.Combine(dir, pkg.EntryPoint)))
                                        {
                                            // Load it
                                            loadedPackage = new ZephyrLoadedPackage(Path.Combine(dir, pkg.EntryPoint));
                                        }
                                    }
                                }
                            }
                        }

                        // Check if it was null
                        if (loadedPackage == null)
                        {
                            Logger.Warning($"Failed to load package {dep.Key}", "package-loader");
                            continue;
                        }

                        // Add to packages
                        Program.LoadedPackages.Add(dep.Key, loadedPackage);
                        Debug.Log($"Loaded package reference {dep.Key} successfully!", "package-loader");
                    }
                }
            }
        }
    }
}
