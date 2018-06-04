using ChatApp.Lib.Messaging.Model;
using ChatApp.Lib.Messaging.Persistence;
using System;
using Xunit;

namespace ChatApp.Lib.Tests.Messaging
{
    public class ChatMessageTableEntityTests
    {
        [Fact]
        public void ChatMessageTableEntity_ValuesSetFromChatMessageCorrectly()
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Handle = "handle",
                Message = "message",
                Timestamp = DateTimeOffset.Now
            };

            var tableEntity = new ChatMessageTableEntity(message);

            Assert.Equal(message.Id, tableEntity.Id);
            Assert.Equal(message.Handle, tableEntity.Handle);
            Assert.Equal(message.Message, tableEntity.Message);
            Assert.Equal(message.Timestamp, tableEntity.Timestamp);
        }

        [Fact]
        public void ChatMessageTableEntity_HandleIsPartitionKey()
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Handle = "handle",
                Message = "message",
                Timestamp = DateTimeOffset.Now
            };

            var tableEntity = new ChatMessageTableEntity(message);

            Assert.Equal(message.Handle, tableEntity.PartitionKey);
        }

        [Fact]
        public void ChatMessageTableEntity_TimestampReversedToRowKey()
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Handle = "handle",
                Message = "message",
                Timestamp = DateTimeOffset.Now
            };

            var tableEntity = new ChatMessageTableEntity(message);

            Assert.Equal(string.Format("{0:D19}", DateTime.MaxValue.Ticks - message.Timestamp.Ticks), tableEntity.PartitionKey);
        }
    }
}