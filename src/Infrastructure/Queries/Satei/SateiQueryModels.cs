namespace Infrastructure.Queries.Satei;

/// <summary>SATEI_Q001 の結果行（サンプル）。</summary>
public sealed class SateiListRow
{
    public int Id { get; init; }
    public string AssessmentNo { get; init; } = string.Empty;
    public string TargetName { get; init; } = string.Empty;
}
