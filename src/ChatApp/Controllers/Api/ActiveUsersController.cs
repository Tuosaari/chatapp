using ChatApp.Lib.Users.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Api
{
    /// <summary>
    /// Controller for providing <see cref="User"/> handles of active users
    /// </summary>
    [Route("api/v1/users")]
    public class ActiveUsersController : Controller
    {
        private readonly IActiveUserRepository _activeUserRepository;

        public ActiveUsersController(IActiveUserRepository activeUserRepository)
        {
            _activeUserRepository = activeUserRepository;
        }

        /// <summary>
        /// Get all active <see cref="User"/> handles
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{String}"/> containing all active user handles (including user him/herself).
        /// List is empty if no active users exist
        /// </returns>
        [HttpGet("active")]
        public async Task<IEnumerable<string>> GetAllActive()
        {
            var users = await _activeUserRepository.GetActiveUsers();
            return users.Select(m => m.Handle);
        }
    }
}
