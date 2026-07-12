namespace Infrastructure.Interfaces;

/// <summary>
/// 汎用リポジトリ（書込みホワイトリストのみ）。
/// 画面読取（一覧・検索・DTO）は IQueryGateway + Queries Catalog を使うこと。
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
}
