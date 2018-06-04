using ChatApp.Lib.Azure.Storage;
using ChatApp.Lib.Users.Model;
using ChatApp.Lib.Users.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChatApp.Lib.Tests.Users
{
    public class AzureStorageActiveUserRepositoryTests
    {
        private readonly Mock<ITableStorageClient> _clientMock;
        private readonly AzureStorageActiveUserRepository _repository;

        public AzureStorageActiveUserRepositoryTests()
        {
            _clientMock = new Mock<ITableStorageClient>();
            _repository = new AzureStorageActiveUserRepository(_clientMock.Object);
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_AddActiveUserCallsClientCorrectly()
        {
            var user = new User
            {
                Id = "id",
                Handle = "handle"
            };

            await _repository.AddActiveUser(user);

            _clientMock.Verify(c => c.InsertOrReplace(It.Is<UserTableEntity>(e => e.Handle == user.Handle && e.Id == user.Id)));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_NullUserShouldThrow()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddActiveUser(null));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_NullUserIdShouldThrow()
        {
            var user = new User
            {
                Id = null,
                Handle = "handle"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddActiveUser(user));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_NullUserHandleShouldThrow()
        {
            var user = new User
            {
                Id = "id",
                Handle = null
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddActiveUser(user));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_GetHandleForIdReturnsHandle()
        {
            var handle = "handle";
            IEnumerable<UserTableEntity> entities = new List<UserTableEntity> {
                new UserTableEntity()
                {
                    Id = "id",
                    Handle = handle
                }
            };

            _clientMock.Setup(c => c.GetByPartitionKey<UserTableEntity>("id")).Returns(Task.FromResult(entities));

            var result = await _repository.GetHandleForId("id");

            Assert.Equal(handle, result);
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_GetHandleForIdReturnsNullIfNotFound()
        {
            IEnumerable<UserTableEntity> entities = new List<UserTableEntity>();
            _clientMock.Setup(c => c.GetByPartitionKey<UserTableEntity>("id")).Returns(Task.FromResult(entities));
            var result = await _repository.GetHandleForId("id");

            Assert.Null(result);
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_GetHandleForIdNullThrows()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.GetHandleForId(null));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_RemoveUserReturnsHandle()
        {
            var entity = new UserTableEntity()
            {
                Id = "id",
                Handle = "handle"
            };

            _clientMock.Setup(c => c.RemoveEntity<UserTableEntity>("id", String.Empty)).Returns(Task.FromResult(entity));

            var result = await _repository.RemoveUser("id");

            Assert.Equal(entity.Handle, result);
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_RemoveUserIdNullThrows()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.RemoveUser(null));
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_ClearActiveUsersCallsClient()
        {
            await _repository.ClearActiveUsers();
            _clientMock.Verify(c => c.RemoveAll());
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_GetActiveUsersReturnsCorrectUsers()
        {
            IEnumerable<UserTableEntity> entities = new List<UserTableEntity> {
                new UserTableEntity()
                {
                    Id = "id",
                    Handle = "handle"
                },
                new UserTableEntity()
                {
                    Id = "id2",
                    Handle = "handle2"
                }
            };

            _clientMock.Setup(c => c.GetAll<UserTableEntity>(It.IsAny<int>())).Returns(Task.FromResult(entities));

            var result = (await _repository.GetActiveUsers()).ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("id", result.First().Id);
            Assert.Equal("id2", result.Skip(1).First().Id);
        }

        [Fact]
        public async Task AzureStorageActiveUserRepository_InitializeCallsClient()
        {
            await _repository.Initialize();
            _clientMock.Verify(c => c.RemoveAll());
        }
    }
}