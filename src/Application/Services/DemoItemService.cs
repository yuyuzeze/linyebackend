using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class DemoItemService : IDemoItemService
{
    private readonly IDemoItemRepository _repository;

    public DemoItemService(IDemoItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DemoItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<DemoItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);
        return item is null ? null : MapToDto(item);
    }

    public async Task<DemoItemDto> CreateAsync(CreateDemoItemDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new DemoItem
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<DemoItemDto?> UpdateAsync(int id, UpdateDemoItemDto dto, CancellationToken cancellationToken = default)
    {
        var updated = await _repository.UpdateAsync(id, item =>
        {
            item.Name = dto.Name;
            item.Description = dto.Description;
        }, cancellationToken);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    private static DemoItemDto MapToDto(DemoItem item) =>
        new(item.Id, item.Name, item.Description, item.CreatedAt);
}
