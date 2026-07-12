using Infrastructure.DataAccess;
using Infrastructure.Queries.Hosho;

namespace Api.Services.Satei;

/// <summary>
/// サンプル：Satei 画面から Hosho の QueryKey を跨業務参照する（SQL はコピーしない）。
/// Controller 未接続。呼び出し形態の参考用。
/// </summary>
public sealed class SateiSampleService
{
    private readonly IQueryGateway _queries;

    public SateiSampleService(IQueryGateway queries) => _queries = queries;

    public Task<IReadOnlyList<HoshoListRow>> SearchGuaranteesForAssessmentAsync(
        CancellationToken cancellationToken = default)
        => _queries.QueryAsync<HoshoListRow>(
            nameof(HoshoQueries.HOSHO_Q001),
            HoshoQueries.HOSHO_Q001,
            cancellationToken: cancellationToken);
}
