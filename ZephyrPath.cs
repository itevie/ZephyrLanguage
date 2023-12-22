using System.IO;

namespace ZephyrNew
{
    internal class ZephyrPath
    {
        public static string Resolve(string current, string given)
        {
            current = current.Replace($"/", Path.DirectorySeparatorChar.ToString()).Replace($"\\", Path.DirectorySeparatorChar.ToString());
            given = given.Replace($"/", Path.DirectorySeparatorChar.ToString()).Replace($"\\", Path.DirectorySeparatorChar.ToString());

            // Check if given is absolute
            if (!given.StartsWith("."))
                return given;
            else return Path.Combine(current, given);
        }
    }
}
