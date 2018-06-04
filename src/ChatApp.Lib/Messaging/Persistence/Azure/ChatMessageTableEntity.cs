using ChatApp.Lib.Messaging.Model;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ChatApp.Lib.Messaging.Persistence
{
    /// <summary>
    /// Class extending <see cref="TableEntity"/> used to store a single <see cref="ChatMessage"/> in table storage
    /// </summary>
    public class ChatMessageTableEntity : TableEntity
    {
        public Guid Id { get; set; }
        public string Handle { get; set; }
        public string Message { get; set; }
        public DateTimeOffset MessageTimestamp { get; set; }

        /// <summary>
        /// Default empty constructor required by storage
        /// </summary>
        public ChatMessageTableEntity()
        {

        }

        /// <summary>
        /// Constructor for creating a table entity of <see cref="ChatMessage"/> directly
        /// </summary>
        /// <param name="message"><see cref="TableEntity"/> to "wrap"</param>
        public ChatMessageTableEntity(ChatMessage message)
        {
            this.Id = message.Id;
            this.Handle = message.Handle;
            this.Message = message.Message;
            this.MessageTimestamp = message.Timestamp;

            //Using handle as row key to eventually support easy(er) message retrieval of a single user/handle
            this.RowKey = message.Handle;

            //"Reversing" timestamp ticks so that PartitionKey ordering provides message in chronological order automatically
            this.PartitionKey = TimestampToPartitionKey(message.Timestamp);
        }

        /// <summary>
        /// Get this entity as <see cref="ChatMessage"/>
        /// </summary>
        /// <returns><see cref="ChatMessage"/> populated with entity values</returns>
        public ChatMessage ToChatMessage()
        {
            return new ChatMessage
            {
                Id = this.Id,
                Handle = this.Handle,
                Message = this.Message,
                Timestamp = this.MessageTimestamp
            };
        }

        /// <summary>
        /// Helper for "reversing" timestamp
        /// </summary>
        private string TimestampToPartitionKey(DateTimeOffset timestamp)
        {
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - timestamp.Ticks);
        }
    }
}
