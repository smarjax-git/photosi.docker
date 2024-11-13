using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PhotoSi.Catalog.Controllers;
using PhotoSi.Catalog.Models;

namespace Test.PhotoSi.Catalog
{
    public class CategorieControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public CategorieControllerTest(TestDatabaseFixture fixture) {
            Database = fixture;
        }

        public TestDatabaseFixture Database { get; set; }

        [Fact]
        public async Task GetCategoria_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var inactive = await context.Categorie.Where(x => x.Active == "N").FirstOrDefaultAsync();

            if (inactive != null)
            {
                var response = await categorieController.GetCategoria(inactive.Id);

                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
        }

        [Fact]
        public async Task GetCategorie_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var categorie = (await categorieController.GetCategorie()).Value;

            Assert.False(categorie.Where(x => x.Active == "N").Any());
        }

        [Fact]
        public async Task GetCategorieReturnsValidData()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var categorie = (await categorieController.GetCategorie()).Value;

            Assert.NotNull(categorie);
            Assert.True(categorie.Count() > 0);
        }

        [Fact]
        public async Task PostCategorie_Returns_BadRequestWithDuplicatedCategoria()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var newCategoria = await context.Categorie.FirstAsync();

            //* L'id resta uno già esistente
            newCategoria.Name = "Nuova categoria";

            var response = await categorieController.PostCategoria(newCategoria);

            Assert.NotNull(response.Result);
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task PostCategorie_Returns_CreatedAtActionResult()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var newCategoria = new Categoria()
            {
                Id = Guid.NewGuid(),
                Name = "Nuova",
                Active = "S"
            };

            var response = await categorieController.PostCategoria(newCategoria);

            Assert.NotNull(response.Result);
            Assert.IsType<CreatedAtActionResult>(response.Result);
            Assert.Equal(((CreatedAtActionResult)response.Result).Value, newCategoria);
        }

        [Fact]
        public async Task PutCategorie_Returns_BadRequest_if_id_not_the_same()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var existingCategoria = await context.Categorie.FirstAsync();

            existingCategoria.Name = "Nuovo nome";

            var response = await categorieController.PutCategoria(Guid.NewGuid(), existingCategoria);

            Assert.NotNull(response);
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task PutCategorie_Returns_NoContent()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var existingCategoria = await context.Categorie.FirstAsync();

            existingCategoria.Name = "Nuovo nome";

            var response = await categorieController.PutCategoria(existingCategoria.Id, existingCategoria);

            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task DeleteCategoria_Returns_NotFound_if_doesnt_exist()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var response = await categorieController.DeleteCategoria(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteCategoria_set_active_N()
        {
            using var context = Database.CreateContext();

            CategorieController categorieController = new CategorieController(context);

            var categoria = await context.Categorie.FirstAsync();

            var response = await categorieController.DeleteCategoria(categoria.Id);

            var response2 = await categorieController.GetCategoria(categoria.Id);

            Assert.Null(response2.Value);
            Assert.IsType<NotFoundResult>(response2.Result);
        }
    }
}