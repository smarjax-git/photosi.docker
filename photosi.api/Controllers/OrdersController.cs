using Microsoft.AspNetCore.Mvc;

using photosi.ws.orders;

using PhotoSi.API.Models;

using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace PhotoSi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly Iorders_ws orders_ws;

        public OrdersController(Iorders_ws orders_ws)
        {
            this.orders_ws = orders_ws;
        }

        [HttpGet("GetAllOrdini/{userId}")]
        [ProducesResponseType(typeof(List<Ordine>), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<Ordine>>> GetAllOrdini(Guid userId)
        {
            try
            {
                var ordini = await orders_ws.OrdiniAllAsync(userId);

                return Ok(ordini);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        [HttpPost("CreaOrdine")]
        [ProducesResponseType(typeof(Ordine), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Ordine>> CreaOrdine(OrdineCreaDTO model)
        { 
            try
            {
                var result = await orders_ws.OrdiniPOSTAsync(
                                                    new Command()
                                                    {
                                                        UserId = model.UserId,
                                                        PickupPointId = model.PickupPointId
                                                    });

                var ordine = result.Value;

                return CreatedAtAction("GetOrdine", new { userId = ordine.UserId, orderId = ordine.Id }, ordine);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        [HttpPost("AggiungiRiga")]
        [ProducesResponseType(typeof(RigaOrdine), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RigaOrdine>> AggiungiRiga(RigaCreaDTO model)
        {
            try
            {
                var rigaCreata = await orders_ws.OrdiniRigheAsync(
                                                new RigaCreationDto()
                                                {
                                                    OrdineId = model.OrdineId,
                                                    UserId = model.UserId,
                                                    ProdottoId = model.ProdottoId,
                                                    Quantita = model.Quantita,
                                                    Prezzo = model.Prezzo
                                                });

                return CreatedAtAction("GetRigheOrdine", new { userId = model.UserId, orderId = model.OrdineId }, rigaCreata);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRigheOrdine")]
        public async Task<ActionResult<List<RigaOrdine>>> GetRigheOrdine(Guid userId, Guid orderId)
        {
            try
            {
                var righe = await orders_ws.GetRigheAsync(userId, orderId);

                 return Ok(righe);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
