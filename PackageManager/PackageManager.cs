using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.PackageManager
{
    internal class RepositoryClient
    {
        public const string DefaultRepository = "https://zephyrrepository.itevie.repl.co";

        public static void InstallPackage(string packageName, string packageVersion, Uri repository)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            Console.WriteLine($"Downloading package {packageName}@{packageVersion} from {repository}");
            ZephyrPackageWithLocation package = GetZephyrPackage();

            // Create client
            HttpClient httpClient = new();

            // Check if latest
            if (packageVersion == "@latest")
            {
                // Fetch latest version
                string u = $"{repository}package/{packageName}/details";
                Console.WriteLine($"Fetching package's latest version from {u}");

                string returnedContent = RepositoryHTTP.Get(u, "installing package");
                ZephyrPackageInformation packageInfo = JsonConvert.DeserializeObject<ZephyrPackageInformation>(returnedContent);

                // Success
                Console.WriteLine($"Latest version is {packageInfo.LatestVersion}");
                packageVersion = packageInfo.LatestVersion;
            }

            string url = $"{repository}package/{packageName}/{packageVersion}/download";
            HttpResponseMessage packageData = RepositoryHTTP.Get(url, "installing package", true);

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

            using (Stream stream = packageData.Content.ReadAsStream())
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
            Console.WriteLine($"Updating {Config.PackageManagerFileName}");
            package.Package.Dependencies.Add(packageName, new()
            {
                Version = packageVersion
            });

            // Save
            File.WriteAllText(Path.Combine(package.Location, Config.PackageManagerFileName), JsonConvert.SerializeObject(package.Package, Formatting.Indented));
            Console.WriteLine($"Done in {stopwatch.ElapsedMilliseconds}ms!");
        }

        public static void UploadPackage(Uri repositoryUrl, string username, string password)
        {
            ZephyrPackageWithLocation package = GetZephyrPackage();

            // Fetch all files
            List<string> files = new();

            foreach (string file in Directory.EnumerateFiles(package.Location, "*.*", SearchOption.AllDirectories)
                .Where(s => !(Path.GetDirectoryName(s).Contains("zephyr_packages"))))
            {
                files.Add(file);
            }

            long totalSize = 0;

            Console.WriteLine($"Found {files.Count} files to upload");
            Console.WriteLine($"\n============ {files.Count} files ============");
            files.ForEach(x =>
            {
                long len = new FileInfo(x).Length;
                totalSize += len;
                Console.WriteLine($"{x.Replace(package.Location, "")} ({len}b)");
            });
            Console.WriteLine($"============ {files.Count} files ============\n");
            Console.WriteLine($"Total unzipped upload size will be {totalSize}b");
            Console.WriteLine($"\nYou are uploading {package.Package.Name}@{package.Package.Version} to {repositoryUrl}!");
            Console.Write($"Confirm upload, use -y to confirm in the future (y/n): ");
            string? cres = Console.ReadLine();

            // Check if user confirmed
            if (cres != "y")
            {
                Environment.Exit(0);
                return;
            }

            // Create ZIP file
            string zipfileLocation = $"{Path.Combine(package.Location, Guid.NewGuid().ToString())}.zip";
            Console.WriteLine($"Generating ZIP file... ({zipfileLocation})");

            using (ZipArchive archive = ZipFile.Open(zipfileLocation, ZipArchiveMode.Create))
            {
                files.ForEach(file =>
                {
                    string name = file.Replace("\\", "/").Replace(package.Location.Replace("\\", "/") + "/", "").Replace(package.Location.Replace("\\", "/"), "");
                    archive.CreateEntryFromFile(file, name);
                    Console.WriteLine($"Added {file} ({name}) to archive");
                });
            }

            Console.WriteLine($"Uploading archive to {repositoryUrl} ({new FileInfo(zipfileLocation).Length}b)");
            string zipData = Convert.ToHexString(File.ReadAllBytes(zipfileLocation));

            Console.WriteLine($"Cleaning up...");
            File.Delete(zipfileLocation);

            // Send request
            RepositoryHTTP.Post($"{repositoryUrl}package/{package.Package.Name}/{package.Package.Version}/upload", new
            {
                username,
                password,
                data = zipData
            }, $"uploading {package.Package.Name}");

            // Done
            Console.WriteLine($"\n{package.Package.Name}@{package.Package.Version} uploaded successfully!");
            Console.WriteLine($"\nDownload using zephyr install-package {package.Package.Name} {package.Package.Version}");
            Console.WriteLine($"Or navigate to {repositoryUrl}package/{package.Package.Name}/{package.Package.Version}");
        }

        public static ZephyrPackageWithLocation GetZephyrPackage()
        {
            Console.WriteLine($"Finding {Config.PackageManagerFileName} in closest directory...");

            string currentDirectory = Directory.GetCurrentDirectory();

            // Check for package.json
            if (File.Exists(Path.Combine(currentDirectory, Config.PackageManagerFileName)) == false)
            {
                Console.WriteLine($"Error: File {Config.PackageManagerFileName} not found! try zephyr new or zephyr init".Pastel(ConsoleColor.Red));
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
            string packageJson = File.ReadAllText(Path.Combine(currentDirectory, Config.PackageManagerFileName));

            ZephyrPackageWithLocation pack = new();

            ZephyrPackage? package = JsonConvert.DeserializeObject<ZephyrPackage>(packageJson);

            if (package == null)
            {
                Console.WriteLine($"Error: Failed to read {Config.PackageManagerFileName}! Check for any errors".Pastel(ConsoleColor.Red));
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

    internal class ZephyrPackageInformation
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("author_id")]
        public long AuthorID { get; set; } = 0;

        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; } = "";
    }
}
