namespace Infrastructure.Queries.Kyotsu;

/// <summary>KYOTSU_Q001 の結果行。</summary>
public sealed class DemoItemListRow
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>KYOTSU_Q002 の結果行。</summary>
public sealed class ActiveUserRoleRow
{
    public int Id { get; init; }
    public string EntraObjectId { get; init; } = string.Empty;
    public string Upn { get; init; } = string.Empty;
    public string RoleCode { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string? DepartmentCode { get; init; }
    public string? DepartmentName { get; init; }
}
