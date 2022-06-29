using System.Threading.Tasks;
using System.Collections.Generic;
using Refit;
using Apizr;

namespace Apizr.Tools.Generator.CLI
{
    [WebApi]
    public interface IUserService
    {
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <returns>successful operation</returns>
        [Post("user/createWithArray")]
        Task CreateUsersWithArrayInputAsync([Body] IEnumerable<User> body);

        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="body">List of user object</param>
        /// <returns>successful operation</returns>
        [Post("user/createWithList")]
        Task CreateUsersWithListInputAsync([Body] IEnumerable<User> body);

        /// <summary>
        /// Get user by user name
        /// </summary>
        /// <param name="username">The name that needs to be fetched. Use user1 for testing.</param>
        /// <returns>successful operation</returns>
        [Get("user/{username}")]
        Task<User> GetUserByNameAsync(string username);

        /// <summary>
        /// Updated user
        /// </summary>
        /// <param name="username">name that need to be updated</param>
        /// <param name="body">Updated user object</param>
        [Put("user/{username}")]
        Task UpdateUserAsync(string username, [Body] User body);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="username">The name that needs to be deleted</param>
        [Delete("user/{username}")]
        Task DeleteUserAsync(string username);

        /// <summary>
        /// Logs user into the system
        /// </summary>
        /// <param name="username">The user name for login</param>
        /// <param name="password">The password for login in clear text</param>
        /// <returns>successful operation</returns>
        [Get("user/login")]
        Task<string> LoginUserAsync([Query] string username, [Query] string password);

        /// <summary>
        /// Logs out current logged in user session
        /// </summary>
        /// <returns>successful operation</returns>
        [Get("user/logout")]
        Task LogoutUserAsync();

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="body">Created user object</param>
        /// <returns>successful operation</returns>
        [Post("user")]
        Task CreateUserAsync([Body] User body);

    }
}