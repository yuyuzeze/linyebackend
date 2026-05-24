using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IVoucherRepository
{
    Task AddRangeAsync(IEnumerable<Voucher> entities, CancellationToken cancellationToken = default);
}
