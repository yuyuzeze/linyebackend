using Api.Interfaces;
using Infrastructure.Interfaces;

namespace Api.Services;

public class CsvMappingService : ICsvMappingService
{
    private readonly ICsvColumnMappingRepository _mappingRepo;

    public CsvMappingService(ICsvColumnMappingRepository mappingRepo) => _mappingRepo = mappingRepo;

    public async Task<IReadOnlyDictionary<string, string>> MapRowToFieldsAsync(int applicationTypeId, IReadOnlyList<string> csvRowValues, CancellationToken cancellationToken = default)
    {
        var mappings = await _mappingRepo.GetByApplicationTypeIdAsync(applicationTypeId, cancellationToken);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var m in mappings)
        {
            if (m.CsvColumnIndex >= 0 && m.CsvColumnIndex < csvRowValues.Count)
            {
                var value = csvRowValues[m.CsvColumnIndex] ?? "";
                result[m.TargetFieldCode] = value;
            }
        }
        return result;
    }
}
