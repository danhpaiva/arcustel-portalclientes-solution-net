using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ArcusTel.PortalClientes.Api.Controllers;

[ApiController]
[Route("api/dids")]
public class DidsController : ControllerBase
{
    private readonly IDidOrchestrator _orchestrator;

    public DidsController(IDidOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] ActivateDidRequest dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.E164Number))
            return BadRequest("E164Number required");

        var normalized = await _orchestrator.ActivateAsync(
            dto.E164Number,
            dto.UserId ?? 0,
            ct
        );

        return Ok(normalized);
    }



    [HttpGet("{requestId}/status")]
    public async Task<IActionResult> GetStatus(long requestId, CancellationToken ct)
    {
        var normalized = await _orchestrator.GetStatusAsync(requestId, ct);
        return Ok(normalized);
    }
}
