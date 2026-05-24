using Api.Models.Dtos;
using Api.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Api.Services;

public class ApplicationTypeService : IApplicationTypeService
{
    private readonly IApplicationTypeRepository _typeRepo;
    private readonly IApplicationTypeFieldRepository _fieldRepo;
    private readonly ICsvColumnMappingRepository _mappingRepo;

    public ApplicationTypeService(
        IApplicationTypeRepository typeRepo,
        IApplicationTypeFieldRepository fieldRepo,
        ICsvColumnMappingRepository mappingRepo)
    {
        _typeRepo = typeRepo;
        _fieldRepo = fieldRepo;
        _mappingRepo = mappingRepo;
    }

    public async Task<IReadOnlyList<ApplicationTypeDto>> GetAllTypesAsync(CancellationToken cancellationToken = default)
    {
        var list = await _typeRepo.GetAllAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<ApplicationTypeDto?> GetTypeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _typeRepo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<ApplicationTypeFieldDto>> GetFieldsAsync(int applicationTypeId, CancellationToken cancellationToken = default)
    {
        var list = await _fieldRepo.GetByApplicationTypeIdAsync(applicationTypeId, cancellationToken);
        return list.Select(f => new ApplicationTypeFieldDto(f.Id, f.ApplicationTypeId, f.FieldCode, f.FieldName, f.DataType, f.DisplayOrder, f.IsRequired)).ToList();
    }

    public async Task<IReadOnlyList<CsvColumnMappingDto>> GetCsvMappingsAsync(int applicationTypeId, CancellationToken cancellationToken = default)
    {
        var list = await _mappingRepo.GetByApplicationTypeIdAsync(applicationTypeId, cancellationToken);
        return list.Select(m => new CsvColumnMappingDto(m.Id, m.ApplicationTypeId, m.CsvColumnIndex, m.CsvColumnName, m.TargetFieldCode)).ToList();
    }

    private static ApplicationTypeDto Map(ApplicationType e) =>
        new(e.Id, e.Code, e.Name, e.Description, e.DisplayOrder);
}
