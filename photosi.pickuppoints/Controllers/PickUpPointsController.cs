using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSi.PickupPoint.Data;
using PhotoSi.PickupPoint.Models;

namespace PhotoSi.PickupPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickUpPointsController : ControllerBase
    {
        private readonly PickUpPointDbContext _context;

        public PickUpPointsController(PickUpPointDbContext context)
        {
            _context = context;
        }

        // GET: api/PickUpPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PickUpPoint>>> GetPickUpPoints()
        {
            return await _context.PickUpPoints.Where(x => x.Active == "S").ToListAsync();
        }

        // GET: api/PickUpPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PickUpPoint>> GetPickUpPoint(Guid id)
        {
            var pickUpPoint = await _context.PickUpPoints.Where(x => x.Active == "S" && x.Id == id).SingleOrDefaultAsync();

            if (pickUpPoint == null)
            {
                return NotFound();
            }

            return pickUpPoint;
        }

        // PUT: api/PickUpPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPickUpPoint(Guid id, PickUpPoint pickUpPoint)
        {
            if (id != pickUpPoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(pickUpPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickUpPointExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PickUpPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<PickUpPoint>), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PickUpPoint>> PostPickUpPoint(PickUpPoint pickUpPoint)
        {
            if (PickUpPointExists(pickUpPoint.Id))
            {
                return BadRequest();
            }

            _context.PickUpPoints.Add(pickUpPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPickUpPoint", new { id = pickUpPoint.Id }, pickUpPoint);
        }

        // DELETE: api/PickUpPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePickUpPoint(Guid id)
        {
            var pickUpPoint = await _context.PickUpPoints.FindAsync(id);

            if (pickUpPoint == null)
            {
                return NotFound();
            }

            pickUpPoint.Active = "N";

            _context.Entry(pickUpPoint).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PickUpPointExists(Guid id)
        {
            return _context.PickUpPoints.Any(e => e.Id == id);
        }
    }
}
