using Microsoft.AspNetCore.Mvc;

using photosi.ws.catalog;

namespace PhotoSi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        private readonly Icatalog_ws ws;

        public ProdottiController(Icatalog_ws ws)
        {
            this.ws = ws;
        }

        [HttpPost("CreateProdotto")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProdotto(Prodotto model)
        { 
            try
            {
                var prodotto = await ws.ProdottiPOSTAsync(model);

                return CreatedAtAction("GetProdotto", new { Id = model.Id }, prodotto);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetProdotti")]
        public async Task<ActionResult<List<Prodotto>>> GetProdotti()
        {
            try
            {
                return Ok(await ws.ProdottiAllAsync());
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetProdotto/{id}")]
        public async Task<ActionResult<List<Prodotto>>> GetProdotti(Guid id)
        {
            try
            {
                return Ok(await ws.ProdottiGETAsync(id));
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
