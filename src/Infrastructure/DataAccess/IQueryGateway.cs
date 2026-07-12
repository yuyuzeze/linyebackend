namespace Infrastructure.DataAccess;

/// <summary>
/// 画面読取 SQL の唯一の入口。書込みは Repository（EF）を使い、本インターフェース経由では行わない。
/// </summary>
public interface IQueryGateway
{
    Task<IReadOnlyList<T>> QueryAsync<T>(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string queryKey,
        string sql,
        object? param = null,
        CancellationToken cancellationToken = default);
}
