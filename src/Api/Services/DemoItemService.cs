using Api.Models.Dtos;
using Api.Interfaces;
using Api.Models;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Api.Services;

public class DemoItemService : IDemoItemService
{
  private const string NotFoundCode = "EKYOTSU40401";
  private const string CreatedCode = "IKYOTSU21001";
  private const string UpdatedCode = "IKYOTSU21002";
  private const string DeletedCode = "IKYOTSU21003";

  private readonly IDemoItemRepository _repository;

  public DemoItemService(IDemoItemRepository repository) => _repository = repository;

  public async Task<ServiceResult<IReadOnlyList<DemoItemDto>>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var items = await _repository.GetAllAsync(cancellationToken);
    return ServiceResult<IReadOnlyList<DemoItemDto>>.Success(items.Select(MapToDto).ToList());
  }

  public async Task<ServiceResult<DemoItemDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var item = await _repository.GetByIdAsync(id, cancellationToken);
    return item is null
      ? ServiceResult<DemoItemDto>.NotFound("指定された DemoItem が見つかりません。", NotFoundCode)
      : ServiceResult<DemoItemDto>.Success(MapToDto(item));
  }

  public async Task<ServiceResult<DemoItemDto>> CreateAsync(CreateDemoItemDto dto, CancellationToken cancellationToken = default)
  {
    var entity = new DemoItem
    {
      Name = dto.Name,
      Description = dto.Description,
      CreatedAt = DateTime.UtcNow
    };
    var created = await _repository.AddAsync(entity, cancellationToken);
    return ServiceResult<DemoItemDto>.Success(
      MapToDto(created),
      StatusCodes.Status201Created,
      new ApiMessageItem(CreatedCode, "DemoItem を登録しました。"));
  }

  public async Task<ServiceResult<DemoItemDto>> UpdateAsync(int id, UpdateDemoItemDto dto, CancellationToken cancellationToken = default)
  {
    var updated = await _repository.UpdateAsync(id, item =>
    {
      item.Name = dto.Name;
      item.Description = dto.Description;
    }, cancellationToken);

    return updated is null
      ? ServiceResult<DemoItemDto>.NotFound("指定された DemoItem が見つかりません。", NotFoundCode)
      : ServiceResult<DemoItemDto>.Success(
          MapToDto(updated),
          StatusCodes.Status200OK,
          new ApiMessageItem(UpdatedCode, "DemoItem を更新しました。"));
  }

  public async Task<ServiceResult<object?>> DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var deleted = await _repository.DeleteAsync(id, cancellationToken);
    return deleted
      ? ServiceResult<object?>.Success(
          null,
          StatusCodes.Status204NoContent,
          new ApiMessageItem(DeletedCode, "DemoItem を削除しました。"))
      : ServiceResult<object?>.NotFound("指定された DemoItem が見つかりません。", NotFoundCode);
  }

  private static DemoItemDto MapToDto(DemoItem item) =>
    new(item.Id, item.Name, item.Description, item.CreatedAt);
}
