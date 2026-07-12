namespace Infrastructure.Queries.Kyotsu;

/// <summary>KYOTSU 担当が管理する画面読取 SQL Catalog。</summary>
public static class KyotsuQueries
{
    /// <summary>DemoItem 一覧</summary>
    public static readonly string KYOTSU_Q001 = """
        SELECT
            Id,
            Name,
            Description,
            CreatedAt
        FROM dbo.DemoItems
        ORDER BY Id
        """;

    /// <summary>有効ユーザーロール一覧（部署名付き）</summary>
    public static readonly string KYOTSU_Q002 = """
        SELECT
            ur.Id,
            ur.EntraObjectId,
            ur.Upn,
            ur.RoleCode,
            ur.IsActive,
            d.Code AS DepartmentCode,
            d.Name AS DepartmentName
        FROM dbo.UserRoles ur
        LEFT JOIN dbo.Departments d ON d.Id = ur.DepartmentId
        WHERE ur.IsActive = 1
        ORDER BY ur.Id
        """;

    /// <summary>Entra ObjectId による有効ユーザーロール取得（部署名付き）</summary>
    public static readonly string KYOTSU_Q003 = """
        SELECT
            ur.Id,
            ur.EntraObjectId,
            ur.Upn,
            ur.RoleCode,
            ur.IsActive,
            d.Code AS DepartmentCode,
            d.Name AS DepartmentName
        FROM dbo.UserRoles ur
        LEFT JOIN dbo.Departments d ON d.Id = ur.DepartmentId
        WHERE ur.IsActive = 1
          AND ur.EntraObjectId = @EntraObjectId
        ORDER BY ur.Id
        """;
}
