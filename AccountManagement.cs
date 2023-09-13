using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr
{
    /// <summary>
    /// This class is used for interacting with the repository.
    /// It is used for account management, such as registering.
    /// </summary>
    internal class AccountManagement
    {
        /// <summary>
        /// Registers a new user to the repository
        /// </summary>
        /// <param name="username">The username of the new user</param>
        /// <param name="password">The password</param>
        /// <param name="repositoryUrl">The repository to register to</param>
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
