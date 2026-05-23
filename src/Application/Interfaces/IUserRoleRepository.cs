using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<UserRole>> GetActiveByObjectIdAsync(string entraObjectId, CancellationToken cancellationToken = default);
}
