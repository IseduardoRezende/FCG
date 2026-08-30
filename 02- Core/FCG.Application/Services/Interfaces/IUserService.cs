using FCG.Application.DTOs.Users;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Filters;

namespace FCG.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<ReadUserDto>> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);

    Task<Result<TokenDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);

    Task<Result<Pagination<ReadUserDto>>> GetPagedAsync(UserFilter filter, CancellationToken cancellationToken = default);

    Task<Result<ReadUserDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Result<ReadUserDto>> UpdateAsync(long id, UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
