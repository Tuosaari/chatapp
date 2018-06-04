using ChatApp.Lib.Azure.Storage;
using ChatApp.Lib.Messaging.Model;
using ChatApp.Lib.Messaging.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChatApp.Lib.Tests.Messaging
{
    public class AzureStorageChatMessageRepositoryTests
    {
        private readonly Mock<ITableStorageClient> _clientMock;
        private readonly AzureStorageChatMessageRepository _repository;

        public AzureStorageChatMessageRepositoryTests()
        {
            _clientMock = new Mock<ITableStorageClient>();
            _repository = new AzureStorageChatMessageRepository(_clientMock.Object);
        }

        [Fact]
        public async Task AzureStorageChatMessageRepository_GetMessagesReturnsMessages()
        {
            IEnumerable<ChatMessageTableEntity> entities = new List<ChatMessageTableEntity> {
                new ChatMessageTableEntity()
                {
                    Id = Guid.NewGuid(),
                    Message = "1"
                },
                new ChatMessageTableEntity()
                {
                    Id = Guid.NewGuid(),
                    Message = "2"
                }
            };

            _clientMock.Setup(c => c.GetAll<ChatMessageTableEntity>(It.IsAny<int>())).Returns(Task.FromResult(entities));

            var result = (await _repository.GetMessages()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("1", result.First().Message);
            Assert.Equal("2", result.Skip(1).First().Message);
        }

        [Fact]
        public async Task AzureStorageChatMessageRepository_InsertMessageNullMessageShouldThrow()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.InsertMessage(null));
        }

        [Fact]
        public async Task AzureStorageChatMessageRepository_InsertMessageNullHandleShouldThrow()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.InsertMessage(
                new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    Handle = null
                }));
        }

        [Fact]
        public async Task AzureStorageChatMessageRepository_InsertMessageEmptyIdShouldThrow()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _repository.InsertMessage(
                new ChatMessage
                {
                    Id = Guid.Empty,
                    Handle = "Handle"
                }));
        }
    }
}