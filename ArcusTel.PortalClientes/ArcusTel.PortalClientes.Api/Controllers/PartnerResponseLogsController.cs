using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArcusTel.PortalClientes.Api.Data;
using ArcusTel.PortalClientes.Api.Models;

namespace ArcusTel.PortalClientes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerResponseLogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PartnerResponseLogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerResponseLog>>> GetPartnerResponseLog()
        {
            return await _context.TB_PartnerResponseLog.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerResponseLog>> GetPartnerResponseLog(long id)
        {
            var partnerResponseLog = await _context.TB_PartnerResponseLog.FindAsync(id);

            if (partnerResponseLog == null)
            {
                return NotFound();
            }

            return partnerResponseLog;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartnerResponseLog(long id, PartnerResponseLog partnerResponseLog)
        {
            if (id != partnerResponseLog.LogId)
            {
                return BadRequest();
            }

            _context.Entry(partnerResponseLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerResponseLogExists(id))
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
        public async Task<ActionResult<PartnerResponseLog>> PostPartnerResponseLog(PartnerResponseLog partnerResponseLog)
        {
            _context.TB_PartnerResponseLog.Add(partnerResponseLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartnerResponseLog", new { id = partnerResponseLog.LogId }, partnerResponseLog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartnerResponseLog(long id)
        {
            var partnerResponseLog = await _context.TB_PartnerResponseLog.FindAsync(id);
            if (partnerResponseLog == null)
            {
                return NotFound();
            }

            _context.TB_PartnerResponseLog.Remove(partnerResponseLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartnerResponseLogExists(long id)
        {
            return _context.TB_PartnerResponseLog.Any(e => e.LogId == id);
        }
    }
}
