using ArcusTel.PortalClientes.Api.Controllers;
using ArcusTel.PortalClientes.Api.DTO;
using ArcusTel.PortalClientes.Api.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ArcusTel.PortalClientes.Api.Tests.Controllers;

public class DidsControllerTests
{
    private readonly Mock<IDidOrchestrator> _orchestratorMock;
    private readonly DidsController _controller;

    public DidsControllerTests()
    {
        _orchestratorMock = new Mock<IDidOrchestrator>();
        _controller = new DidsController(_orchestratorMock.Object);
    }

    #region Activate Tests

    [Fact]
    public async Task Activate_ShouldReturnBadRequest_WhenE164NumberIsNullOrEmpty()
    {
        var dto = new ActivateDidRequest { E164Number = "", UserId = 1 };
        var result = await _controller.Activate(dto, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("e164Number", badRequest.Value.ToString());
    }

    [Fact]
    public async Task Activate_ShouldReturnBadRequest_WhenUserIdIsNull()
    {
        var dto = new ActivateDidRequest { E164Number = "+5511999999999", UserId = null };
        var result = await _controller.Activate(dto, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("userId", badRequest.Value.ToString());
    }

    [Fact]
    public async Task Activate_ShouldReturnInternalServerError_WhenOrchestratorThrows()
    {
        var dto = new ActivateDidRequest { E164Number = "+5511999999999", UserId = 1 };
        _orchestratorMock
            .Setup(x => x.ActivateAsync(dto.E164Number, dto.UserId.Value, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro do Orchestrator"));

        var result = await _controller.Activate(dto, CancellationToken.None);

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
        Assert.Contains("Falha ao ativar", status.Value.ToString());
    }

    [Fact]
    public async Task Activate_ShouldReturnOk_WhenActivationSucceeds()
    {
        var dto = new ActivateDidRequest { E164Number = "+5511999999999", UserId = 1 };

        var normalizedResponse = new NormalizedActivationResponse(
            ExternalId: "123",
            DidNumber: dto.E164Number,
            PartnerId: default,
            Status: Enum.DidStatus.Active,
            DetailMessage: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        _orchestratorMock
            .Setup(x => x.ActivateAsync(dto.E164Number, dto.UserId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(normalizedResponse);

        var result = await _controller.Activate(dto, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<NormalizedActivationResponse>(okResult.Value);

        Assert.Equal(normalizedResponse.ExternalId, returned.ExternalId);
        Assert.Equal(normalizedResponse.Status, returned.Status);
        Assert.Equal(normalizedResponse.DidNumber, returned.DidNumber);
        Assert.Equal(normalizedResponse.PartnerId, returned.PartnerId);
    }

    #endregion

    #region GetStatus Tests

    [Fact]
    public async Task GetStatus_ShouldReturnOk_WithStatus()
    {
        long requestId = 100;

        var statusResponse = new NormalizedActivationResponse(
            ExternalId: "100",
            DidNumber: "+5511999999999",
            PartnerId: default,
            Status: Enum.DidStatus.Active,
            DetailMessage: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        _orchestratorMock
            .Setup(x => x.GetStatusAsync(requestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusResponse);

        var result = await _controller.GetStatus(requestId, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<NormalizedActivationResponse>(okResult.Value);

        Assert.Equal(statusResponse.ExternalId, returned.ExternalId);
        Assert.Equal(statusResponse.Status, returned.Status);
    }

    #endregion
}