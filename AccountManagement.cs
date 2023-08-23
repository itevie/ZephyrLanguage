using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    internal class AccountManagement
    {
        public static void Register(string username, string password, string repositoryUrl)
        {
            RepositoryHTTP.Post($"{repositoryUrl}/users/register", new
            {
                username,
                password
            }, "registering");

            Console.WriteLine($"Account created!");
        }
    }
}
