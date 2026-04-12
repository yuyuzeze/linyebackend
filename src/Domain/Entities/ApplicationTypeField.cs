namespace Domain.Entities;

public class ApplicationTypeField
{
    public int Id { get; set; }
    public int ApplicationTypeId { get; set; }
    public string FieldCode { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string DataType { get; set; } = "string";
    public int DisplayOrder { get; set; }
    public bool IsRequired { get; set; }
}
