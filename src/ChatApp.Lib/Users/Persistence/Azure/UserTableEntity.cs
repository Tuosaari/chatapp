using ChatApp.Lib.Users.Model;
using Microsoft.WindowsAzure.Storage.Table;

namespace ChatApp.Lib.Users.Persistence
{
    /// <summary>
    /// Class extending <see cref="TableEntity"/> used to store a single <see cref="User"/> in table storage
    /// </summary>
    public class UserTableEntity : TableEntity
    {
        public string Id { get; set; }
        public string Handle { get; set; }

        /// <summary>
        /// Default empty constructor required by storage
        /// </summary>
        public UserTableEntity()
        {

        }

        /// <summary>
        /// Constructor for creating a table entity of <see cref="User"/> directly
        /// </summary>
        /// <param name="user"><see cref="User"/> to "wrap"</param>
        public UserTableEntity(User user)
        {
            this.Id = user.Id;
            this.Handle = user.Handle;
            this.PartitionKey = user.Id;
            this.RowKey = string.Empty;
        }

        /// <summary>
        /// Get this entity as <see cref="User"/>
        /// </summary>
        /// <returns><see cref="User"/> populated with entity values</returns>
        public User ToUser()
        {
            return new User
            {
                Handle = this.Handle,
                Id = this.Id
            };
        }
    }
}
