using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly AppDbContext _db;

    public UserRoleRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<UserRole>> GetActiveByObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default)
    {
        return await _db.UserRoles
            .Include(x => x.Department)
            .Where(x => x.EntraObjectId == entraObjectId && x.IsActive)
            .ToListAsync(cancellationToken);
    }
}
