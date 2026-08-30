using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<UserRole>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);

    Task<UserRole?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
