using ChatApp.Lib.Users.Model;
using ChatApp.Lib.Users.Persistence;
using Xunit;

namespace ChatApp.Lib.Tests.Users
{
    public class UserTableEntityTests
    {
        [Fact]
        public void UserTableEntity_ValuesSetFromUserCorrectly()
        {
            var user = new User
            {
                Id = "id",
                Handle = "handle"
            };

            var tableEntity = new UserTableEntity(user);

            Assert.Equal(user.Id, tableEntity.Id);
            Assert.Equal(user.Handle, tableEntity.Handle);
        }

        [Fact]
        public void UserTableEntity_IdIsPartitionKey()
        {
            var user = new User
            {
                Id = "id",
                Handle = "handle"
            };

            var tableEntity = new UserTableEntity(user);

            Assert.Equal(user.Id, tableEntity.PartitionKey);
        }
    }
}