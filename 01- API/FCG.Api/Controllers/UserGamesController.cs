using FCG.Api.Extensions;
using FCG.Application.DTOs.UserGames;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FCG.Api.Controllers;

[Route("api/v{version:apiVersion}/user-games")]
public class UserGamesController : BaseController
{
    private readonly IUserGameService _userGameService;

    public UserGamesController(IUserGameService userGameService)
    {
        _userGameService = userGameService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Purchase a game")]
    [ProducesResponseType(typeof(SuccessResult<ReadUserGameDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> PurchaseAsync([FromBody] CreateUserGameDto dto, CancellationToken cancellationToken)
    {
        return (await _userGameService.PurchaseAsync(dto, cancellationToken)).ToActionResult(StatusCodes.Status201Created);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "List user game library")]
    [ProducesResponseType(typeof(SuccessResult<IReadOnlyList<ReadUserGameDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLibraryAsync([FromQuery] long? userId, CancellationToken cancellationToken)
    {
        return (await _userGameService.GetLibraryAsync(userId, cancellationToken)).ToActionResult();
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get purchased game by id")]
    [ProducesResponseType(typeof(SuccessResult<ReadUserGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        return (await _userGameService.GetByIdAsync(id, cancellationToken)).ToActionResult();
    }
}
