using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.Models;

namespace ArcusTel.PortalClientes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerStatusMappingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnerStatusMappingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerStatusMapping>>> GetPartnerStatusMapping()
        {
            return await _context.TB_PartnerStatusMapping.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerStatusMapping>> GetPartnerStatusMapping(long id)
        {
            var partnerStatusMapping = await _context.TB_PartnerStatusMapping.FindAsync(id);

            if (partnerStatusMapping == null)
            {
                return NotFound();
            }

            return partnerStatusMapping;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartnerStatusMapping(long id, PartnerStatusMapping partnerStatusMapping)
        {
            if (id != partnerStatusMapping.Id)
            {
                return BadRequest();
            }

            _context.Entry(partnerStatusMapping).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerStatusMappingExists(id))
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
        public async Task<ActionResult<PartnerStatusMapping>> PostPartnerStatusMapping(PartnerStatusMapping partnerStatusMapping)
        {
            _context.TB_PartnerStatusMapping.Add(partnerStatusMapping);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartnerStatusMapping", new { id = partnerStatusMapping.Id }, partnerStatusMapping);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartnerStatusMapping(long id)
        {
            var partnerStatusMapping = await _context.TB_PartnerStatusMapping.FindAsync(id);
            if (partnerStatusMapping == null)
            {
                return NotFound();
            }

            _context.TB_PartnerStatusMapping.Remove(partnerStatusMapping);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartnerStatusMappingExists(long id)
        {
            return _context.TB_PartnerStatusMapping.Any(e => e.Id == id);
        }
    }
}
