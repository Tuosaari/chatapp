using ChatApp.Lib.Azure.Storage;
using ChatApp.Lib.Messaging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Lib.Messaging.Persistence
{
    /// <summary>
    /// Azure Table Storage based implementation of <see cref="IChatMessageRepository"/> 
    /// </summary>
    public class AzureStorageChatMessageRepository : IChatMessageRepository
    {
        private readonly ITableStorageClient _client;

        public AzureStorageChatMessageRepository(ITableStorageClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ChatMessage>> GetMessages(int maxCount = 0)
        {
            //RowKey is "reverse" timestamp => returned entities or in latest to oldest order automatically
            var entities = await _client.GetAll<ChatMessageTableEntity>(maxCount).ConfigureAwait(false);
            return entities.Select(e => e.ToChatMessage());
        }

        /// <inheritdoc />
        public async Task InsertMessage(ChatMessage message)
        {
            if (message == null) {
                throw new ArgumentNullException("Message cannot be null", nameof(message));
            }

            if (message.Id == Guid.Empty) {
                throw new ArgumentException(nameof(message.Id), "Message Id cannot be empty Guid");
            }

            if (string.IsNullOrWhiteSpace(message.Handle)) {
                throw new ArgumentNullException("Message handle cannot be null or white space", nameof(message.Handle));
            }

            await _client.InsertOrReplace(new ChatMessageTableEntity(message));
        }
    }
}
