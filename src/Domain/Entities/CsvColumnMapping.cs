namespace Domain.Entities;

public class CsvColumnMapping
{
    public int Id { get; set; }
    public int ApplicationTypeId { get; set; }
    public int CsvColumnIndex { get; set; }
    public string? CsvColumnName { get; set; }
    public string TargetFieldCode { get; set; } = string.Empty;
}
