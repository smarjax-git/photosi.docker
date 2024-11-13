using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.Catalog.Controllers;
using PhotoSi.Catalog.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.PhotoSi.Catalog
{
    public class ProdottiControllerTest : IClassFixture<TestDatabaseFixture>
    {
        public ProdottiControllerTest(TestDatabaseFixture fixture)
        {
            Database = fixture;
        }

        public TestDatabaseFixture Database { get; set; }

        [Fact]
        public async Task GetProdotto_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var inactive = await context.Prodotti.Where(x => x.Active == "N").FirstOrDefaultAsync();

            if (inactive != null)
            {
                var response = await prodottiController.GetProdotto(inactive.Id);

                Assert.Null(response.Value);
                Assert.IsType<NotFoundResult>(response.Result);
            }
        }

        [Fact]
        public async Task GetProdotti_ReturnsOnlyActive()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var prodotti = (await prodottiController.GetProdotti()).Value;

            Assert.False(prodotti.Where(x => x.Active == "N").Any());
        }

        [Fact]
        public async Task GetProdottiReturnsValidData()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var prodotti = (await prodottiController.GetProdotti()).Value;

            Assert.NotNull(prodotti);
        }

        [Fact]
        public async Task PostProdotto_Returns_BadRequest_WithDuplicatedProdotto()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var newProdotto = await context.Prodotti.FirstAsync();

            //* L'id resta uno già esistente
            newProdotto.Name = "Nuovo prodotto";

            var response = await prodottiController.PostProdotto(newProdotto);

            Assert.NotNull(response.Result);
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task PostProdotto_Returns_BadRequest_WithNotValidCategoriaId()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var newProdotto = new Prodotto()
            {
                Id = Guid.NewGuid(),
                Active = "S",
                CategoriaId = Guid.NewGuid(),
                Name = "NUOVO",
                Codice = "NUOVOCODICE",
                Price = 10m
            };

            var response = await prodottiController.PostProdotto(newProdotto);

            Assert.NotNull(response.Result);
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task PostProdotto_Returns_CreatedAtActionResult()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var categoria = await context.Categorie.FirstAsync();

            var newProdotto = new Prodotto()
            {
                Id = Guid.NewGuid(),
                Codice = "NUOVOCODICE",
                Name = "Nuova",
                Active = "S",
                CategoriaId = categoria.Id,
                Price = 9m
            };

            var response = await prodottiController.PostProdotto(newProdotto);

            Assert.NotNull(response.Result);
            Assert.IsType<CreatedAtActionResult>(response.Result);
            Assert.Equal(((CreatedAtActionResult)response.Result).Value, newProdotto);
        }

        [Fact]
        public async Task PutProdotto_Returns_BadRequest_if_id_not_the_same()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var existingProdotto = await context.Prodotti.FirstAsync();

            existingProdotto.Name = "Nuovo nome";

            var response = await prodottiController.PutProdotto(Guid.NewGuid(), existingProdotto);

            Assert.NotNull(response);
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task PutProdotto_Returns_BadRequest_if_not_valid_categoriaid()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var existingProdotto = await context.Prodotti.FirstAsync();

            var test = new Prodotto()
            {
                Id = existingProdotto.Id,
                Active = existingProdotto.Active,
                CategoriaId = Guid.NewGuid(),
                Codice = existingProdotto.Codice,
                Name = existingProdotto.Name,
                Price = existingProdotto.Price
            };

            var response = await prodottiController.PutProdotto(existingProdotto.Id, test);

            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task PutProdotti_Returns_NoContent()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var existingProdotto = await context.Prodotti.FirstAsync();

            existingProdotto.Name = "Nuovo nome";

            var response = await prodottiController.PutProdotto(existingProdotto.Id, existingProdotto);

            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task DeleteProdotto_Returns_NotFound_if_doesnt_exist()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var response = await prodottiController.DeleteProdotto(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task DeleteProdotto_set_active_N()
        {
            using var context = Database.CreateContext();

            ProdottiController prodottiController = new ProdottiController(context);

            var prodotto = await context.Prodotti.FirstAsync();

            var response = await prodottiController.DeleteProdotto(prodotto.Id);

            var response2 = await prodottiController.GetProdotto(prodotto.Id);

            Assert.Null(response2.Value);
            Assert.IsType<NotFoundResult>(response2.Result);
        }
    }
}
