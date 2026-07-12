namespace Infrastructure.Queries.Hosho;

/// <summary>HOSHO 担当が管理する画面読取 SQL Catalog。</summary>
public static class HoshoQueries
{
    /// <summary>保証一覧検索（サンプル：テーブル未整備のため空結果の形のみ）</summary>
    public static readonly string HOSHO_Q001 = """
        SELECT
            CAST(NULL AS INT)            AS Id,
            CAST(N'' AS NVARCHAR(100))   AS GuaranteeNo,
            CAST(N'' AS NVARCHAR(200))   AS ApplicantName
        WHERE 1 = 0
        """;
}
