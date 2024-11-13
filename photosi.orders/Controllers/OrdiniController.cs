using MediatR;

using Microsoft.AspNetCore.Mvc;

using PhotoSi.Orders.Models;

using Create = PhotoSi.Orders.Features.Ordini.Create;
using Index = PhotoSi.Orders.Features.Ordini.Index;

namespace PhotoSi.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdiniController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Ordini
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Ordine>>> GetOrdini(Guid userId)
        {
            return await _mediator.Send(new Index.Query { UserId = userId });
        }

        [HttpGet("{userId}/{orderId}")]
        public async Task<ActionResult<Ordine>> GetOrdine(Guid userId, Guid orderId)
        {
            var ordini = await _mediator.Send(new Index.Query { UserId = userId, OrderId = orderId });

            if (ordini == null || ordini.Count == 0)
            {
                return NotFound();
            }

            return ordini[0];
        }

        [HttpPost]
        [ProducesResponseType(typeof(Ordine), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Ordine>> PostOrdine(Create.Command model)
        {
            try
            {
                var ordine = await _mediator.Send(model);

                return CreatedAtAction("GetOrdine", new { userId = model.UserId, orderId = ordine.Id }, ordine);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
