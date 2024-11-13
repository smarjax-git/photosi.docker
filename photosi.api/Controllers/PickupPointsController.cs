using Microsoft.AspNetCore.Mvc;

using photosi.api.Models;
using photosi.ws.pickuppoints;


namespace PhotoSi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickupPointsController : ControllerBase
    {
        private readonly Ipickuppoints_ws ws;

        public PickupPointsController(Ipickuppoints_ws ws)
        {
            this.ws = ws;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ActionResult<PickUpPoint>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PickUpPoint>> Create(CreatePickupPointDto model)
        { 
            try
            {
                var pickuppoint = (await ws.PickUpPointsPOSTAsync(new PickUpPoint
                {  
                    Id = Guid.NewGuid(),
                    Active = "S",
                    Address = model.Address,
                    City = model.City,
                    Name = model.Name,
                    ZipCode = model.ZipCode
                })).Value;

                return CreatedAtAction("Get", new { id = pickuppoint.Id }, pickuppoint);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<List<PickUpPoint>>> GetAll()
        {
            try
            {
                return Ok(await ws.PickUpPointsAllAsync());
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<PickUpPoint>> Get(Guid id)
        {
            try
            {
                return Ok(await ws.PickUpPointsGETAsync(id));
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
