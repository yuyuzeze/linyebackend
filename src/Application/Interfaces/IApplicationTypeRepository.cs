using Domain.Entities;

namespace Application.Interfaces;

public interface IApplicationTypeRepository
{
    Task<IReadOnlyList<ApplicationType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApplicationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
