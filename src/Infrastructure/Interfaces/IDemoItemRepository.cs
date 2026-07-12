using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

/// <summary>DemoItem の書込み経路（ホワイトリスト）。一覧などの画面読取は KyotsuQueries + IQueryGateway。</summary>
public interface IDemoItemRepository
{
    Task<DemoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DemoItem> AddAsync(DemoItem entity, CancellationToken cancellationToken = default);
    Task<DemoItem?> UpdateAsync(int id, Action<DemoItem> update, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
