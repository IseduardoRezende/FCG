using FCG.Application.DTOs.Users;
using FCG.Domain.Commons.Result;

namespace FCG.Application.Services.Interfaces;

public interface IUserRoleService
{
    Task<Result<IReadOnlyList<ReadUserRoleDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
