using FCG.Api.Extensions;
using FCG.Application.DTOs.Games;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons.Result;
using FCG.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FCG.Api.Controllers;

[Route("api/v{version:apiVersion}/games")]
public class GamesController : BaseController
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "Create game")]
    [ProducesResponseType(typeof(SuccessResult<ReadGameDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGameDto dto, CancellationToken cancellationToken)
    {
        return (await _gameService.CreateAsync(dto, cancellationToken)).ToActionResult(StatusCodes.Status201Created);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "List games")]
    [ProducesResponseType(typeof(SuccessResult<IReadOnlyList<ReadGameDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _gameService.GetAllAsync(cancellationToken)).ToActionResult();
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get game by id")]
    [ProducesResponseType(typeof(SuccessResult<ReadGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        return (await _gameService.GetByIdAsync(id, cancellationToken)).ToActionResult();
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "Update game")]
    [ProducesResponseType(typeof(SuccessResult<ReadGameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] UpdateGameDto dto, CancellationToken cancellationToken)
    {
        return (await _gameService.UpdateAsync(id, dto, cancellationToken)).ToActionResult();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "Delete game")]
    [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        return (await _gameService.DeleteAsync(id, cancellationToken)).ToActionResult();
    }
}
