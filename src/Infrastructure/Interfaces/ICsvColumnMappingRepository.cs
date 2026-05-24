using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface ICsvColumnMappingRepository
{
    Task<IReadOnlyList<CsvColumnMapping>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default);
}
