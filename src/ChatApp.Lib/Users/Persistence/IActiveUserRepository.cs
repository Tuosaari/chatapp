using ChatApp.Lib.Users.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Lib.Users.Persistence
{
    /// <summary>
    /// Repository for currently active <see cref="User">Users</see>
    /// </summary>
    public interface IActiveUserRepository
    {
        /// <summary>
        /// Add new active <see cref="User"/>.
        /// Note: any previous active users with the same id will be replaced.
        /// </summary>
        /// <param name="user"><see cref="User"/> to add to active users</param>
        Task AddActiveUser(User user);

        /// <summary>
        /// Get handle matching the id provided
        /// </summary>
        /// <param name="id">Id of the <see cref="User"/></param>
        /// <returns>Handle associated with the id, or null if not found</returns>
        Task<string> GetHandleForId(string id);

        /// <summary>
        /// Remove active <see cref="User"/> with given id
        /// </summary>
        /// <param name="id">Id of the <see cref="User"/> to remove</param>
        /// <returns>Handle of the removed user, or null if id didn't match any active <see cref="User"/></returns>
        Task<string> RemoveUser(string id);

        /// <summary>
        /// Clear all active <see cref="User">users</see> in repository
        /// </summary>
        Task ClearActiveUsers();

        /// <summary>
        /// Get all active <see cref="User">users</see> available in this repository.
        /// </summary>
        /// <returns><see cref="IEnumerable{User}"/>, which is empy if no users are active</returns>
        Task<IEnumerable<User>> GetActiveUsers();
    }
}
