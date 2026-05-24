using Api.Models.Dtos;

namespace Api.Interfaces;

public interface IApplicationTypeService
{
    Task<IReadOnlyList<ApplicationTypeDto>> GetAllTypesAsync(CancellationToken cancellationToken = default);
    Task<ApplicationTypeDto?> GetTypeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApplicationTypeFieldDto>> GetFieldsAsync(int applicationTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CsvColumnMappingDto>> GetCsvMappingsAsync(int applicationTypeId, CancellationToken cancellationToken = default);
}
