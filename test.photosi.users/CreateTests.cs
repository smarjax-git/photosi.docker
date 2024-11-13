using PhotoSi.Users.Features.Users;
using PhotoSi.Users.Models;

using Shouldly;

//using Create = PhotoSi.Users.Features.Utenti.Create;

namespace Test.PhotoSi.Users
{
    [Collection(nameof(Fixture))]
    public class CreateTests
    {

        private readonly Fixture _fixture;

        public CreateTests(Fixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_create_user()
        {
            var cmd = new Create.Command
            {
                Name = "Michael",
                Surname = "Jackson",
                Email = "michael.jackson@gmail.com"
            };

            var createdUser = await _fixture.SendAsync(cmd);

            var searchUser = await _fixture.FindAsync<User>(createdUser.Id);

            searchUser.ShouldNotBeNull();
            searchUser.Email.ShouldBe(cmd.Email);
            searchUser.Name.ShouldBe(cmd.Name);
            searchUser.Surname.ShouldBe(cmd.Surname);
        }
    }
}