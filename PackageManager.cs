using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class PackageManager
    {
        public const string DefaultRepository = "https://zephyrrepository.itevie.repl.co";

        public static void InstallPackage(string packageName, string packageVersion, Uri repository)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine($"Downloading package {packageName}@{packageVersion} from {repository}");
            ZephyrPackageWithLocation package = GetZephyrPackage();

            // Create client
            HttpClient httpClient = new HttpClient();

            // Check if latest
            if (packageVersion == "@latest")
            {
                // Fetch latest version
                string u = $"{repository}package/{packageName}/latest/version";
                Console.WriteLine($"Fetching package's latest version from {u}"); 
                
                HttpResponseMessage r;

                try
                {
                    // Try GET
                    r = httpClient.GetAsync(u).GetAwaiter().GetResult();
                    Console.WriteLine($"Recieved status code {r.StatusCode} ({(r.IsSuccessStatusCode ? "Success" : "Failure")})");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Fatal HTTP error: {e.Message}, try again later!".Pastel(ConsoleColor.Red));
                    Environment.Exit(0);
                    return;
                }

                // Check if successful
                if (!r.IsSuccessStatusCode)
                {
                    // Get result
                    string jsonRes = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    // Get message
                    PackageManagerErrorResult? errRes = JsonConvert.DeserializeObject<PackageManagerErrorResult>(jsonRes);
                    if (errRes == null) errRes.Message = "Unknown error";

                    Console.WriteLine($"Error[{(int)r.StatusCode}] occurred whilst installing package: {errRes.Message}".Pastel(ConsoleColor.Red));
                    Environment.Exit(0);
                }

                string returnedContent = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Success
                Console.WriteLine($"Latest version is {returnedContent}");
                packageVersion = returnedContent;
            }

            string url = $"{repository}package/{packageName}/{packageVersion}/download";
            Console.WriteLine($"GET {url}");

            HttpResponseMessage res;

            try
            {
                // Try GET
                res = httpClient.GetAsync(url).GetAwaiter().GetResult();
                Console.WriteLine($"Recieved status code {res.StatusCode} ({(res.IsSuccessStatusCode ? "Success" : "Failure")})");
            } catch (HttpRequestException e)
            {
                Console.WriteLine($"Fatal HTTP error: {e.Message}, try again later!".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
                return;
            }

            // Check if successful
            if (!res.IsSuccessStatusCode)
            {
                // Get result
                string jsonRes = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Get message
                PackageManagerErrorResult? errRes = JsonConvert.DeserializeObject<PackageManagerErrorResult>(jsonRes);
                if (errRes == null) errRes.Message = "Unknown error";

                Console.WriteLine($"Error[{(int)res.StatusCode}] occurred whilst installing package: {errRes.Message}".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
            }

            string zephyrPackageFolder = Path.Combine(package.Location, "zephyr_packages");
            string packageFolder = Path.Combine(zephyrPackageFolder, packageName);

            // Check if already installed
            if (package.Package.Dependencies.ContainsKey(packageName))
            {
                // Delete
                Console.WriteLine($"Clearing package {packageName} as it is already installed...");
                package.Package.Dependencies.Remove(packageName);

                try
                {
                    Directory.Delete(packageFolder, true);
                } catch { }
            }

            // Create folder
            Console.WriteLine($"Creating {packageFolder}");
            Directory.CreateDirectory(packageFolder);

            // Create temp fodler
            string packageTempFolder = Path.Combine(packageFolder, "%temp%");
            Console.WriteLine($"Creating temp folder... ({packageTempFolder})");
            Directory.CreateDirectory(packageTempFolder);

            // Save to the temp folder
            string packageZipFile = Path.Combine(packageTempFolder, "temp.zip");
            Console.WriteLine($"Saving downloaded ZIP to {packageZipFile}");

            using (Stream stream = res.Content.ReadAsStream())
            {
                using (Stream zip = File.OpenWrite(packageZipFile))
                {
                    stream.CopyTo(zip);
                }
            }

            // Extract
            Console.WriteLine($"Extracting {packageZipFile} to {packageFolder}");
            ZipFile.ExtractToDirectory(packageZipFile, packageFolder);

            // Cleanup
            Console.WriteLine($"Cleanup...");
            Directory.Delete(packageTempFolder, true);

            // Update package.json
            Console.WriteLine($"Updating package.json");
            package.Package.Dependencies.Add(packageName, new()
            {
                Version = packageVersion
            });

            // Save
            File.WriteAllText(Path.Combine(package.Location, "package.json"), JsonConvert.SerializeObject(package.Package, Formatting.Indented));
            Console.WriteLine($"Done in {stopwatch.ElapsedMilliseconds}ms!");
        }

        public static ZephyrPackageWithLocation GetZephyrPackage()
        {
            Console.WriteLine($"Finding package.json in closest directory...");

            string currentDirectory = Directory.GetCurrentDirectory();

            // Check for package.json
            if (File.Exists(Path.Combine(currentDirectory, "package.json")) == false)
            {
                Console.WriteLine($"Error: File package.json not found! try zephyr new or zephyr init".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
            }

            // Check for zephyr_packages
            if (Directory.Exists(Path.Combine(currentDirectory, "zephyr_packages")) == false)
            {
                Directory.CreateDirectory(Path.Combine(currentDirectory, "zephyr_packages"));
                Console.WriteLine($"Created zephyr_packages folder");
            }
            
            // Found it
            Console.WriteLine($"Folder is {currentDirectory}");

            // Read package.json
            string packageJson = File.ReadAllText(Path.Combine(currentDirectory, "package.json"));

            ZephyrPackageWithLocation pack = new();

            ZephyrPackage? package = JsonConvert.DeserializeObject<ZephyrPackage>(packageJson);

            if (package == null)
            {
                Console.WriteLine($"Error: Failed to read package.json! Check for any errors".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
            }

            pack.Package = package;
            pack.Location = currentDirectory;

            return pack;
        }
    }

    internal class PackageManagerErrorResult
    {
        [JsonProperty("message")]
        public string Message { get; set; } = "";
    }

    internal class ZephyrPackageWithLocation
    {
        public ZephyrPackage Package { get; set; } = new ZephyrPackage();
        public string Location { get; set; } = "";
    }
}
