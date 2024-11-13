using MediatR;

using Microsoft.AspNetCore.Mvc;

using PhotoSi.Users.Features.Users;
using PhotoSi.Users.Models;

using Index = PhotoSi.Users.Features.Users.Index;
using UserCreateCommand = PhotoSi.Users.Features.Users.Create.Command;

namespace PhotoSi.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _mediator.Send(new Index.Query());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = (await _mediator.Send(new Index.Query { Id = id })).SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _mediator.Send(new Edit.Command()
                {
                    Id = id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Active = user.Active,
                    Email = user.Email
                });
            }
            catch(InvalidOperationException ioe)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostUser")]
        [ProducesResponseType(typeof(ActionResult<User>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> PostUser(UserCreateCommand model)
        {
            try
            { 
                var user = await _mediator.Send(model);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _mediator.Send(new Delete.Command
                {
                    Id = id
                });
            }
            catch (InvalidOperationException ioe)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<bool> UserExists(Guid id)
        {
            var user = (await _mediator.Send(new Index.Query { Id = id })).SingleOrDefault();

            return user != null;
        }
    }
}
