using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.Catalog.Data;
using PhotoSi.Catalog.Models;

namespace PhotoSi.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        private readonly CatalogDbContext _context;

        public ProdottiController(CatalogDbContext context)
        {
            _context = context;
        }

        // GET: api/Prodotti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotto>>> GetProdotti()
        {
            return await _context.Prodotti
                                    .Include(x => x.Categoria)
                                    .Where(x => x.Active == "S").ToListAsync();
        }

        // GET: api/Prodotti/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prodotto>> GetProdotto(Guid id)
        {
            var prodotto = await _context.Prodotti
                                            .Include(x => x.Categoria)
                                                .Where(x => x.Id == id && x.Active == "S").SingleOrDefaultAsync();

            if (prodotto == null)
            {
                return NotFound();
            }

            return prodotto;
        }

        // PUT: api/Prodotti/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdotto(Guid id, Prodotto prodotto)
        {
            if (id != prodotto.Id)
            {
                return BadRequest();
            }

            var existing = (await GetProdotto(id)).Value;

            if (existing == null)
            {
                return NotFound();
            }

            if(existing.CategoriaId != prodotto.CategoriaId)
            {
                if(!await _context.Categorie.AnyAsync(c => c.Id == prodotto.CategoriaId))
                {
                    return BadRequest(prodotto.CategoriaId);
                }
            }

            _context.Entry(prodotto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Prodotti
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<Prodotto>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Prodotto>> PostProdotto(Prodotto prodotto)
        {
            if (ProdottoExists(prodotto.Id))
            {
                return BadRequest();
            }

            if (!await _context.Categorie.AnyAsync(c => c.Id == prodotto.CategoriaId))
            {
                return BadRequest(prodotto.CategoriaId);
            }
            
            _context.Prodotti.Add(prodotto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdotto", new { id = prodotto.Id }, prodotto);
        }

        // DELETE: api/Prodotti/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdotto(Guid id)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);

            if (prodotto == null)
            {
                return NotFound();
            }

            prodotto.Active = "N";

            _context.Entry(prodotto).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdottoExists(Guid id)
        {
            return _context.Prodotti.Any(e => e.Id == id);
        }
    }
}
