using FCG.Application.DTOs.Users;
using FCG.Application.Services.Interfaces;
using FCG.Domain.Commons.Result;
using FCG.Domain.Repositories;

namespace FCG.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    public async Task<Result<IReadOnlyList<ReadUserRoleDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _userRoleRepository.GetAllAsync(cancellationToken);

        var result = roles.Select(r => new ReadUserRoleDto
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();

        return SuccessResult<IReadOnlyList<ReadUserRoleDto>>.Create(result);
    }
}
