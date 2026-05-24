using Api.Models.Dtos;
using Api.Models;

namespace Api.Interfaces;

public interface IDemoItemService
{
  Task<ServiceResult<IReadOnlyList<DemoItemDto>>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<ServiceResult<DemoItemDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
  Task<ServiceResult<DemoItemDto>> CreateAsync(CreateDemoItemDto dto, CancellationToken cancellationToken = default);
  Task<ServiceResult<DemoItemDto>> UpdateAsync(int id, UpdateDemoItemDto dto, CancellationToken cancellationToken = default);
  Task<ServiceResult<object?>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
