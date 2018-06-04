using ChatApp.Lib.Azure.Storage;
using ChatApp.Lib.General;
using ChatApp.Lib.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Lib.Users.Persistence
{
    /// <summary>
    /// Azure Table Storage based implementation of <see cref="T:ChatApp.Lib.Users.Persistence.IActiveUserRepository" /> 
    /// </summary>
    public class AzureStorageActiveUserRepository : IActiveUserRepository, IInitializable
    {
        private readonly ITableStorageClient _client;

        public AzureStorageActiveUserRepository(ITableStorageClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task AddActiveUser(User user)
        {
            if (user == null) {
                throw new ArgumentNullException("User cannot be null", nameof(user));
            }

            if (user.Id == null) {
                throw new ArgumentNullException("User Id cannot be null", nameof(user.Id));
            }

            if (user.Handle == null) {
                throw new ArgumentNullException("User Handle cannot be null", nameof(user.Handle));
            }

            await _client.InsertOrReplace(new UserTableEntity(user));
        }

        /// <inheritdoc />
        public async Task<string> GetHandleForId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException("Id cannot be null or white space", nameof(id));
            }

            var entity = await GetUserTableEntity(id).ConfigureAwait(false);
            return entity?.Handle;
        }

        /// <inheritdoc />
        public async Task<string> RemoveUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException("Id cannot be null or white space", nameof(id));
            }

            var entity = await _client.RemoveEntity<UserTableEntity>(id, string.Empty).ConfigureAwait(false);
            return entity?.Handle;
        }

        /// <inheritdoc />
        public async Task ClearActiveUsers()
        {
            await _client.RemoveAll().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetActiveUsers()
        {
            var entities = await _client.GetAll<UserTableEntity>().ConfigureAwait(false);
            return entities.Select(e => e.ToUser());
        }

        /// <summary>
        /// Clear all active users on initialize. Should only be called once on application start up
        /// </summary>
        public async Task Initialize()
        {
            //TODO: Not ideal as this won't work with multiple servers. Refactor if multiple servers are needed
            await _client.RemoveAll();
        }

        private async Task<UserTableEntity> GetUserTableEntity(string id)
        {
            var entities = await _client.GetByPartitionKey<UserTableEntity>(id).ConfigureAwait(false);
            //There should always be only one user with same Id
            return entities.SingleOrDefault();
        }
    }
}
