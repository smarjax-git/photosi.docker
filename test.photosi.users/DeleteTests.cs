using PhotoSi.Users.Features.Users;
using PhotoSi.Users.Models;

using Shouldly;

namespace Test.PhotoSi.Users
{
    [Collection(nameof(Fixture))]
    public class DeleteTests
    {

        private readonly Fixture _fixture;

        public DeleteTests(Fixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_delete_user()
        {
            var cmd = new Create.Command
            {
                Name = "Michael",
                Surname = "Jackson",
                Email = "michael.jackson@gmail.com"
            };

            var createdUser = await _fixture.SendAsync(cmd);

            var deleteCommand = new Delete.Command
            {
                Id = createdUser.Id
            };

            await _fixture.SendAsync(deleteCommand);

            var user = await _fixture.FindAsync<User>(createdUser.Id);

            user.ShouldBeNull();
        }
    }
}