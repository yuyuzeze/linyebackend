using Application.DTOs;

namespace Application.Interfaces;

public interface IDemoItemService
{
    Task<IReadOnlyList<DemoItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DemoItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DemoItemDto> CreateAsync(CreateDemoItemDto dto, CancellationToken cancellationToken = default);
    Task<DemoItemDto?> UpdateAsync(int id, UpdateDemoItemDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
