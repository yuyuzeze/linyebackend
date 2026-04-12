namespace Application.Services.Csv;

/// <summary>
/// CSV 行映射（支持中文或英文表头）.
/// </summary>
public class VoucherCsvRow
{
    public string? VoucherDate { get; set; }
    public string? Summary { get; set; }
    public string? DebitAccount { get; set; }
    public string? CreditAccount { get; set; }
    public string? Amount { get; set; }
}
