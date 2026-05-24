namespace Api.Models.Dtos;

public class AuthMeDto
{
    public string ObjectId { get; set; } = string.Empty;
    public string Upn { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? DepartmentCode { get; set; }
    public string? DepartmentName { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = [];
    public bool AuthEnabled { get; set; }
}
