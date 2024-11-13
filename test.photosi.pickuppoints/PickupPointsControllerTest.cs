using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.PickupPoint.Models;

using PhotoSi.PickupPoint.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Test.PhotoSi.PickupPoints
{
    public class PickupPointsControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public PickupPointsControllerTest(TestDatabaseFixture fixture)
        {
            Database = fixture;
        }

        public TestDatabaseFixture Database { get; set; }

        [Fact]
        public async Task GetPickupPoint_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            var pickupPointsController = new PickUpPointsController(context);

            var inactive = await context.PickUpPoints.Where(x => x.Active == "N").FirstOrDefaultAsync();

            if (inactive != null)
            {
                var response = await pickupPointsController.GetPickUpPoint(inactive.Id);

                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
        }

        [Fact]
        public async Task GetPickupPopints_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            var pickupPointsController = new PickUpPointsController(context);

            var response = await pickupPointsController.GetPickUpPoints();

            Assert.NotNull(response.Value);
            Assert.False(response.Value.Where(x => x.Active == "N").Any());
        }

        [Fact]
        public async Task GetCategorie_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            var pickupPoints = new PickUpPointsController(context);

            var response = await pickupPoints.GetPickUpPoints();

            Assert.NotNull(response.Value);
            Assert.True(response.Value.Count() > 0);
        }

        [Fact]
        public async Task PostPickupPoint_Returns_BadRequestIfDuplicated()
        {
            using var context = Database.CreateContext();

            var pickupPointsController = new PickUpPointsController(context);

            var newPP = await context.PickUpPoints.FirstAsync();

            //* L'id resta uno già esistente
            //* Controllare anche P.IVA 
            newPP.Name = "Nuovo PP";

            var response = await pickupPointsController.PostPickUpPoint(newPP);

            Assert.NotNull(response.Result);
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task PostPickupPoint_Returns_CreatedAtActionResult()
        {
            using var context = Database.CreateContext();

            var pickupPointsController = new PickUpPointsController(context);

            var newPP = new PickUpPoint()
            {
                Id = Guid.NewGuid(),
                Active = "S",
                Address = "Via Roma, 1",
                Name = "Tabaccheria",
                City = "Minerbe",
                ZipCode = "37044"
            };

            var response = await pickupPointsController.PostPickUpPoint(newPP);

            Assert.NotNull(response.Result);
            Assert.IsType<CreatedAtActionResult>(response.Result);
            Assert.Equal(((CreatedAtActionResult)response.Result).Value, newPP);
        }

        public async Task PutPickupPoint_Returns_BadRequest_if_id_not_the_same()
        {
            using var context = Database.CreateContext();

            var pickupController = new PickUpPointsController(context);

            var existing = await context.PickUpPoints.FirstAsync();

            existing.Name = "Nuovo nome";

            var response = await pickupController.PutPickUpPoint(Guid.NewGuid(), existing);

            Assert.NotNull(response);
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task PutPickupPoints_Returns_NoContent()
        {
            using var context = Database.CreateContext();

            var pickupPointController = new PickUpPointsController(context);

            var existing = await context.PickUpPoints.FirstAsync();

            existing.Name = "Nuovo nome";

            var response = await pickupPointController.PutPickUpPoint(existing.Id, existing);

            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task DeletePickupPoints_Returns_NotFound_if_doesnt_exist()
        {
            using var context = Database.CreateContext();

            var pickupPointController = new PickUpPointsController(context);

            var response = await pickupPointController.DeletePickUpPoint(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeletePickupPoints_set_active_N()
        {
            using var context = Database.CreateContext();

            var pickupPointsController = new PickUpPointsController(context);

            var pp = await context.PickUpPoints.FirstAsync();

            var response = await pickupPointsController.DeletePickUpPoint(pp.Id);

            var response2 = await pickupPointsController.GetPickUpPoint(pp.Id);

            Assert.Null(response2.Value);
            Assert.IsType<NotFoundResult>(response2.Result);
        }
    }
}