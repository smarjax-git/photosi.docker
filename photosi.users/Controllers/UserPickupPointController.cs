using MediatR;

using Microsoft.AspNetCore.Mvc;

using photosi.users.Models;

using PhotoSi.Users.Features.UserPickupPoints;
using PhotoSi.Users.Models;

namespace PhotoSi.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPickupPointController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserPickupPointController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/UsePickupPoint
        [HttpGet("GetAll/{userid}")]
        public async Task<ActionResult<IEnumerable<UserPickupPoint>>> GetAll(Guid userId)
        {
            return await _mediator.Send(new Features.UserPickupPoints.Index.Query { UserId = userId });
        }

        // POST: api/UsePickupPoint
        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<UserPickupPoint>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserPickupPoint>> PostUserPickupPoints(CreateUserPickupPointDto model)
        {
            try
            {
                var userPickupPoint = await _mediator.Send(new AddPickupPoint.Command
                {
                    PickupPointId = model.PickupPointId,
                    UserId = model.UserId
                });
                
                return CreatedAtAction("GetAll", new { userId = userPickupPoint.UserId }, userPickupPoint);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
