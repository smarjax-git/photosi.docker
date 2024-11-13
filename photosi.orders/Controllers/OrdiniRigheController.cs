using MediatR;

using Microsoft.AspNetCore.Mvc;

using PhotoSi.Orders.Models;

using PhotoSi.Orders.Features.OrdiniRighe;

using Index = PhotoSi.Orders.Features.OrdiniRighe.Index;
using PhotoSi.Orders.Models.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoSi.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniRigheController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdiniRigheController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetRighe")]
        public async Task<ActionResult<IEnumerable<RigaOrdine>>> GetRighe(Guid userId, Guid orderId)
        {
            return await _mediator.Send(new Index.Query { UserId = userId, OrderId = orderId });
        }

        [HttpPost]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RigaOrdine>> PostRiga(RigaCreationDto riga)
        {
            try
            {
                var res = await _mediator.Send(new Create.Command
                {
                    OrdineId = riga.OrdineId,
                    UserId = riga.UserId,
                    Prezzo = riga.Prezzo,
                    ProdottoId = riga.ProdottoId,
                    Quantita = riga.Quantita
                });

                return CreatedAtAction("GetRighe", new { userId = riga.UserId, orderId = riga.OrdineId }, res);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
