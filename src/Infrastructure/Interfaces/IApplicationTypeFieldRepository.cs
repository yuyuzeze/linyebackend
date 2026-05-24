using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IApplicationTypeFieldRepository
{
    Task<IReadOnlyList<ApplicationTypeField>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default);
}
