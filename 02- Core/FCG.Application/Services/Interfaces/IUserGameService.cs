using FCG.Application.DTOs.UserGames;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Filters;

namespace FCG.Application.Services.Interfaces;

public interface IUserGameService
{
    Task<Result<ReadUserGameDto>> PurchaseAsync(CreateUserGameDto dto, CancellationToken cancellationToken = default);

    Task<Result<Pagination<ReadUserGameDto>>> GetLibraryAsync(UserGameFilter filter, CancellationToken cancellationToken = default);

    Task<Result<ReadUserGameDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
