namespace Application.DTOs;

public record ApplicationTypeDto(int Id, string Code, string Name, string? Description, int DisplayOrder);

public record ApplicationTypeFieldDto(int Id, int ApplicationTypeId, string FieldCode, string FieldName, string DataType, int DisplayOrder, bool IsRequired);

public record CsvColumnMappingDto(int Id, int ApplicationTypeId, int CsvColumnIndex, string? CsvColumnName, string TargetFieldCode);
