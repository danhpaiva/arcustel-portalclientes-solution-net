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
            return BadRequest(new
            {
                Error = "O campo 'e164Number' é obrigatório.",
                Example = "+5511999999999"
            });

        if (!dto.UserId.HasValue)
            return BadRequest(new
            {
                Error = "O campo 'userId' deve ser um número inteiro.",
                Example = 123
            });

        try
        {
            var normalized = await _orchestrator.ActivateAsync(dto.E164Number, dto.UserId.Value, ct);

            return Ok(normalized);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = "Falha ao ativar o DID.",
                Detail = ex.Message
            });
        }
    }

    [HttpGet("{requestId}/status")]
    public async Task<IActionResult> GetStatus(long requestId, CancellationToken ct)
    {
        var normalized = await _orchestrator.GetStatusAsync(requestId, ct);
        return Ok(normalized);
    }
}