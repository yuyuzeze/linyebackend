namespace Domain.Entities;

public class Voucher
{
    public int Id { get; set; }
    public DateTime VoucherDate { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string? DebitAccount { get; set; }
    public string? CreditAccount { get; set; }
    public decimal Amount { get; set; }
    public string? SourceBlobPath { get; set; }
    public DateTime CreatedAt { get; set; }
}
