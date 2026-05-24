namespace Api.Interfaces;

public interface ICsvMappingService
{
    /// <summary>
    /// 指定した申請書種別について、CSV 行（インデックス）をフィールドコード → 値にマッピングする。
    /// </summary>
    Task<IReadOnlyDictionary<string, string>> MapRowToFieldsAsync(int applicationTypeId, IReadOnlyList<string> csvRowValues, CancellationToken cancellationToken = default);
}
