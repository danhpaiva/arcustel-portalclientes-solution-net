using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.Models;

namespace ArcusTel.PortalClientes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DidActivationRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DidActivationRequestsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DidActivationRequest>>> GetDidActivationRequest()
        {
            return await _context.TB_DidActivationRequest.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DidActivationRequest>> GetDidActivationRequest(long id)
        {
            var didActivationRequest = await _context.TB_DidActivationRequest.FindAsync(id);

            if (didActivationRequest == null)
            {
                return NotFound();
            }

            return didActivationRequest;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDidActivationRequest(long id, DidActivationRequest didActivationRequest)
        {
            if (id != didActivationRequest.Id)
            {
                return BadRequest();
            }

            _context.Entry(didActivationRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DidActivationRequestExists(id))
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

        [HttpPost]
        public async Task<ActionResult<DidActivationRequest>> PostDidActivationRequest(DidActivationRequest didActivationRequest)
        {
            _context.TB_DidActivationRequest.Add(didActivationRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDidActivationRequest", new { id = didActivationRequest.Id }, didActivationRequest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDidActivationRequest(long id)
        {
            var didActivationRequest = await _context.TB_DidActivationRequest.FindAsync(id);
            if (didActivationRequest == null)
            {
                return NotFound();
            }

            _context.TB_DidActivationRequest.Remove(didActivationRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DidActivationRequestExists(long id)
        {
            return _context.TB_DidActivationRequest.Any(e => e.Id == id);
        }
    }
}
