using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class RepositoryHTTP
    {
        public static void Post(string url, object data, string action = "posting")
        {
            HttpClient httpClient = new();
            string sendingData = JsonConvert.SerializeObject(data);

            HttpResponseMessage? res = null;
            string? jsonRes = null;

            try
            {
                Console.WriteLine($"POST {url}");
                res = httpClient.PostAsync(url, new StringContent(sendingData, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                Console.WriteLine($"Recieved status code {res.StatusCode} ({(res.IsSuccessStatusCode ? "Success" : "Failure")})");

                // Check if successful
                if (!res.IsSuccessStatusCode)
                {
                    // Get result
                    jsonRes = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    // Get message
                    PackageManagerErrorResult? errRes = JsonConvert.DeserializeObject<PackageManagerErrorResult>(jsonRes);
                    if (errRes == null) errRes.Message = "Unknown error";

                    Console.WriteLine($"Error[{(int)res.StatusCode}] occurred whilst {action}: {errRes.Message}".Pastel(ConsoleColor.Red));
                    Environment.Exit(0);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Fatal HTTP error: {e.Message}, try again later!".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine($"Failed to read response ({e.Message})".Pastel(ConsoleColor.Red));
                Console.WriteLine($"{jsonRes}".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
            }
        }

        public static string Get(string url, string action = "getting")
        {
            HttpClient httpClient = new();

            HttpResponseMessage res;
            string result = "";

            try
            {
                // Try GET
                res = httpClient.GetAsync(url).GetAwaiter().GetResult();
                result = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine($"Recieved status code {res.StatusCode} ({(res.IsSuccessStatusCode ? "Success" : "Failure")})");

                // Check if successful
                if (!res.IsSuccessStatusCode)
                {
                    // Get result
                    string jsonRes = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    // Get message
                    PackageManagerErrorResult? errRes = JsonConvert.DeserializeObject<PackageManagerErrorResult>(jsonRes);
                    if (errRes == null) errRes.Message = "Unknown error";

                    Console.WriteLine($"Error[{(int)res.StatusCode}] occurred whilst {action}: {errRes.Message}".Pastel(ConsoleColor.Red));
                    Environment.Exit(0);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Fatal HTTP error: {e.Message}, try again later!".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
                return "";
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine($"Failed to read response ({e.Message})".Pastel(ConsoleColor.Red));
                Console.WriteLine($"{result}".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
                return "";
            }

            return result;
        }

        public static HttpResponseMessage Get(string url, string action = "getting", bool returnRes = true)
        {
            HttpClient httpClient = new();

            HttpResponseMessage res;

            try
            {
                // Try GET
                res = httpClient.GetAsync(url).GetAwaiter().GetResult();
                Console.WriteLine($"Recieved status code {res.StatusCode} ({(res.IsSuccessStatusCode ? "Success" : "Failure")})");

                // Check if successful
                if (!res.IsSuccessStatusCode)
                {
                    string jsonRes = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    // Get message
                    PackageManagerErrorResult? errRes = JsonConvert.DeserializeObject<PackageManagerErrorResult>(jsonRes);
                    if (errRes == null) errRes.Message = "Unknown error";

                    Console.WriteLine($"Error[{(int)res.StatusCode}] occurred whilst {action}: {errRes.Message}".Pastel(ConsoleColor.Red));
                    Environment.Exit(0);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Fatal HTTP error: {e.Message}, try again later!".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
                return null;
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine($"Failed to read response ({e.Message})".Pastel(ConsoleColor.Red));
                Environment.Exit(0);
                return null;
            }

            return res;
        }
    }
}
