using FCG.Api.Extensions;
using FCG.Application.DTOs.Users;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FCG.Api.Controllers;

[Route("api/v{version:apiVersion}/users")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;

    public UsersController(IUserService userService, IUserRoleService userRoleService)
    {
        _userService = userService;
        _userRoleService = userRoleService;
    }

    [AllowAnonymous]
    [HttpGet("roles")]
    [SwaggerOperation(Summary = "List user roles")]
    [ProducesResponseType(typeof(SuccessResult<IReadOnlyList<ReadUserRoleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRolesAsync(CancellationToken cancellationToken)
    {
        return (await _userRoleService.GetAllAsync(cancellationToken)).ToActionResult();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user")]
    [ProducesResponseType(typeof(SuccessResult<ReadUserDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        return (await _userService.RegisterAsync(dto, cancellationToken)).ToActionResult(StatusCodes.Status201Created);
    }

    [AllowAnonymous]
    [HttpPost("logins")]
    [SwaggerOperation(Summary = "Authenticate user")]
    [ProducesResponseType(typeof(SuccessResult<TokenDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        return (await _userService.LoginAsync(dto, cancellationToken)).ToActionResult();
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "List users")]
    [ProducesResponseType(typeof(SuccessResult<Pagination<ReadUserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return (await _userService.GetPagedAsync(page, pageSize, cancellationToken)).ToActionResult();
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get user by id")]
    [ProducesResponseType(typeof(SuccessResult<ReadUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        if (!CanAccessUser(id))
        {
            return Forbid();
        }

        return (await _userService.GetByIdAsync(id, cancellationToken)).ToActionResult();
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "Update user")]
    [ProducesResponseType(typeof(SuccessResult<ReadUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        return (await _userService.UpdateAsync(id, dto, cancellationToken)).ToActionResult();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = nameof(UserRoles.Administrator))]
    [SwaggerOperation(Summary = "Delete user")]
    [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        return (await _userService.DeleteAsync(id, cancellationToken)).ToActionResult();
    }

    private bool CanAccessUser(long id)
    {
        if (User.IsInRole(nameof(UserRoles.Administrator)))
        {
            return true;
        }

        var claim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(claim, out var userId) && userId == id;
    }
}
