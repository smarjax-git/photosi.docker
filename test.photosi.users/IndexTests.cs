using PhotoSi.Users.Models;

using Shouldly;

using Index = PhotoSi.Users.Features.Users.Index;

namespace Test.PhotoSi.Users
{
    [Collection(nameof(Fixture))]
    public class IndexTests
    {

        private readonly Fixture _fixture;

        public IndexTests(Fixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_return_all_items_for_default_search()
        {
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Michael",
                Surname = "Night",
                Active = "S",
                Email = "michael.might@gmail.com"
            };

            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Michael",
                Surname = "Jordan",
                Active = "S",
                Email = "mj@gmail.com"
            };

            await _fixture.InsertAsync(user1, user2);

            var query = new Index.Query { Id = user1.Id };

            var result = await _fixture.SendAsync(query);

            result.Count.ShouldBe(1);
            result[0].Id.ShouldBeEquivalentTo(user1.Id);
        }
    }
}