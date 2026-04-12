using Domain.Entities;

namespace Application.Interfaces;

public interface IApplicationTypeFieldRepository
{
    Task<IReadOnlyList<ApplicationTypeField>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default);
}
