using Microsoft.AspNetCore.Mvc;

using photosi.ws.users;

namespace PhotoSi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Iusers_ws ws;

        public UsersController(Iusers_ws ws)
        {
            this.ws = ws;
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(typeof(ActionResult<User>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> CreateUser(Command model)
        { 
            try
            {
                var user = (await ws.PostUserAsync(model)).Value;

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }


        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                return Ok(await ws.UsersAllAsync());
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<List<User>>> GetUser(Guid id)
        {
            try
            {
                return Ok(await ws.UsersGETAsync(id));
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }
    }
}
