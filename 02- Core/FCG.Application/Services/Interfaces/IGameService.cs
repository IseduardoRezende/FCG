using FCG.Application.DTOs.Games;
using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FCG.Domain.Filters;

namespace FCG.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task<Result<ReadGameDto>> CreateAsync(CreateGameDto dto, CancellationToken cancellationToken = default);

        Task<Result<Pagination<ReadGameDto>>> GetPagedAsync(GameFilter filter, CancellationToken cancellationToken = default);

        Task<Result<ReadGameDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<ReadGameDto>> UpdateAsync(long id, UpdateGameDto dto, CancellationToken cancellationToken = default);

        Task<Result<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}