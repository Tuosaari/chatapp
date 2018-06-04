using ChatApp.Lib.Messaging.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Lib.Messaging.Persistence
{
    /// <summary>
    /// Repository for <see cref="ChatMessage">chat messages</see>
    /// </summary>
    public interface IChatMessageRepository
    {
        /// <summary>
        /// Get all <see cref="ChatMessage">chat messages</see> available in the repository.
        /// Count of returned messages is implementation specific and can be limited.
        /// </summary>
        /// <param name="maxCount">Maximum count of messages returned, set to 0 to retrieve all. Default is 0</param>
        /// <returns><see cref="IEnumerable{ChatMessage}"/>, which is empy if no messages were found</returns>
        Task<IEnumerable<ChatMessage>> GetMessages(int maxCount = 0);

        /// <summary>
        /// Insert a single <see cref="ChatMessage"/> into the repository
        /// </summary>
        /// <param name="message"><see cref="ChatMessage"/> to insert into repository</param>
        Task InsertMessage(ChatMessage message);
    }
}
