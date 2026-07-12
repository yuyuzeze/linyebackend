namespace Infrastructure.Queries.Hosho;

/// <summary>HOSHO_Q001 の結果行（サンプル）。</summary>
public sealed class HoshoListRow
{
    public int Id { get; init; }
    public string GuaranteeNo { get; init; } = string.Empty;
    public string ApplicantName { get; init; } = string.Empty;
}
