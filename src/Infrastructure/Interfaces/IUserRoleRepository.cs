using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<UserRole>> GetActiveByObjectIdAsync(string entraObjectId, CancellationToken cancellationToken = default);
}
