namespace Infrastructure.Queries.Satei;

/// <summary>SATEI 担当が管理する画面読取 SQL Catalog。</summary>
public static class SateiQueries
{
    /// <summary>査定一覧（サンプル）</summary>
    public static readonly string SATEI_Q001 = """
        SELECT
            CAST(NULL AS INT)            AS Id,
            CAST(N'' AS NVARCHAR(100))   AS AssessmentNo,
            CAST(N'' AS NVARCHAR(200))   AS TargetName
        WHERE 1 = 0
        """;
}
