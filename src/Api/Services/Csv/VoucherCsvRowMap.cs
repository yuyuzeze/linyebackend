using CsvHelper.Configuration;

namespace Api.Services.Csv;

/// <summary>
/// CSV 列を順序でマッピング：列0=日付、列1=摘要、列2=借方科目、列3=貸方科目、列4=金額。
/// </summary>
public sealed class VoucherCsvRowMap : ClassMap<VoucherCsvRow>
{
    public VoucherCsvRowMap()
    {
        Map(m => m.VoucherDate).Index(0).Optional();
        Map(m => m.Summary).Index(1).Optional();
        Map(m => m.DebitAccount).Index(2).Optional();
        Map(m => m.CreditAccount).Index(3).Optional();
        Map(m => m.Amount).Index(4).Optional();
    }
}
