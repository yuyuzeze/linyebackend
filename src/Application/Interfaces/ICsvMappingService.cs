namespace Application.Interfaces;

public interface ICsvMappingService
{
    /// <summary>
    /// Maps a CSV row (by index) to field code -> value for the given application type.
    /// </summary>
    Task<IReadOnlyDictionary<string, string>> MapRowToFieldsAsync(int applicationTypeId, IReadOnlyList<string> csvRowValues, CancellationToken cancellationToken = default);
}
