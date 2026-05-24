using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IApplicationTypeRepository
{
    Task<IReadOnlyList<ApplicationType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApplicationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
