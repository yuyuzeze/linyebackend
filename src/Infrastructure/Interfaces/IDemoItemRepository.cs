using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IDemoItemRepository
{
    Task<IReadOnlyList<DemoItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DemoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DemoItem> AddAsync(DemoItem entity, CancellationToken cancellationToken = default);
    Task<DemoItem?> UpdateAsync(int id, Action<DemoItem> update, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
