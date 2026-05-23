namespace Domain.Entities;

public class UserRole
{
    public int Id { get; set; }
    public string EntraObjectId { get; set; } = string.Empty;
    public string Upn { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
