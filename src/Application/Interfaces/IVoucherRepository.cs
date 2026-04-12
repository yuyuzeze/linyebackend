using Domain.Entities;

namespace Application.Interfaces;

public interface IVoucherRepository
{
    Task AddRangeAsync(IEnumerable<Voucher> entities, CancellationToken cancellationToken = default);
}
