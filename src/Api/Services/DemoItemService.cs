using Api.Models.Dtos;
using Api.Interfaces;
using Api.Models;
using Infrastructure.DataAccess;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Queries.Kyotsu;

namespace Api.Services;

public class DemoItemService : IDemoItemService
{
  private const string NotFoundCode = "EKYOTSU40401";
  private const string CreatedCode = "IKYOTSU21001";
  private const string UpdatedCode = "IKYOTSU21002";
  private const string DeletedCode = "IKYOTSU21003";

  private readonly IRepository<DemoItem> _repository;
  private readonly IQueryGateway _queries;

  public DemoItemService(IRepository<DemoItem> repository, IQueryGateway queries)
  {
    _repository = repository;
    _queries = queries;
  }

  /// <summary>画面一覧 → QueryGateway + KYOTSU_Q001（Repository は使わない）。</summary>
  public async Task<ServiceResult<IReadOnlyList<DemoItemDto>>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var rows = await _queries.QueryAsync<DemoItemListRow>(
      nameof(KyotsuQueries.KYOTSU_Q001),
      KyotsuQueries.KYOTSU_Q001,
      cancellationToken: cancellationToken);

    var dtos = rows
      .Select(r => new DemoItemDto(r.Id, r.Name, r.Description, r.CreatedAt))
      .ToList();

    return ServiceResult<IReadOnlyList<DemoItemDto>>.Success(dtos);
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
    await _repository.AddAsync(entity, cancellationToken);
    return ServiceResult<DemoItemDto>.Success(
      MapToDto(entity),
      StatusCodes.Status201Created,
      new ApiMessageItem(CreatedCode, "DemoItem を登録しました。"));
  }

  public async Task<ServiceResult<DemoItemDto>> UpdateAsync(int id, UpdateDemoItemDto dto, CancellationToken cancellationToken = default)
  {
    var item = await _repository.GetByIdAsync(id, cancellationToken);
    if (item is null)
      return ServiceResult<DemoItemDto>.NotFound("指定された DemoItem が見つかりません。", NotFoundCode);

    item.Name = dto.Name;
    item.Description = dto.Description;
    await _repository.UpdateAsync(item, cancellationToken);

    return ServiceResult<DemoItemDto>.Success(
        MapToDto(item),
        StatusCodes.Status200OK,
        new ApiMessageItem(UpdatedCode, "DemoItem を更新しました。"));
  }

  public async Task<ServiceResult<object?>> DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var item = await _repository.GetByIdAsync(id, cancellationToken);
    if (item is null)
      return ServiceResult<object?>.NotFound("指定された DemoItem が見つかりません。", NotFoundCode);

    await _repository.RemoveAsync(item, cancellationToken);
    return ServiceResult<object?>.Success(
        null,
        StatusCodes.Status200OK,
        new ApiMessageItem(DeletedCode, "DemoItem を削除しました。"));
  }

  private static DemoItemDto MapToDto(DemoItem item) =>
    new(item.Id, item.Name, item.Description, item.CreatedAt);
}
