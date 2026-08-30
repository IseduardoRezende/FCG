using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly FcgDbContext _dbContext;

    public UserRoleRepository(FcgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<UserRole>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserRole>()
            .OrderBy(r => r.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserRole>().AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<UserRole?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserRole>().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
