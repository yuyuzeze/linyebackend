using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VoucherRepository : IVoucherRepository
{
    private readonly AppDbContext _db;

    public VoucherRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddRangeAsync(IEnumerable<Voucher> entities, CancellationToken cancellationToken = default)
    {
        await _db.Vouchers.AddRangeAsync(entities, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
