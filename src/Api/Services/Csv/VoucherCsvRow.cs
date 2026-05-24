namespace Api.Services.Csv;

/// <summary>
/// CSV 行マッピング（中国語または英語ヘッダー対応）。
/// </summary>
public class VoucherCsvRow
{
    public string? VoucherDate { get; set; }
    public string? Summary { get; set; }
    public string? DebitAccount { get; set; }
    public string? CreditAccount { get; set; }
    public string? Amount { get; set; }
}
