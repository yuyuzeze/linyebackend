using Domain.Entities;

namespace Application.Interfaces;

public interface ICsvColumnMappingRepository
{
    Task<IReadOnlyList<CsvColumnMapping>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default);
}
