using PhotoSi.Orders.Models;

using Shouldly;

using Index = PhotoSi.Orders.Features.Ordini.Index;

namespace Test.PhotoSi.Ordini
{
    [Collection(nameof(Fixture))]
    public class IndexTests
    {

        private readonly Fixture _fixture;

        public IndexTests(Fixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_return_all_items_for_default_search()
        {
            var userId = Guid.NewGuid();

            var ordine1 = new Ordine
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                NrOrdine = "1",
                PickupPointId = Guid.NewGuid(),
                Stato = "C",
                UserId = userId
            };

            var ordine2 = new Ordine
            {
                Id = Guid.NewGuid(),
                Data = DateTime.Now,
                NrOrdine = "2",
                PickupPointId = Guid.NewGuid(),
                Stato = "C",
                UserId = userId
            };

            await _fixture.InsertAsync(ordine1, ordine2);

            var query = new Index.Query { UserId = userId };
            
            var result = await _fixture.SendAsync(query);

            result.Count.ShouldBe(2);
            result.Any(x => x.Id == ordine1.Id).ShouldBe(true); 
            result.Any(x => x.Id == ordine2.Id).ShouldBe(true);
        }
    }
}