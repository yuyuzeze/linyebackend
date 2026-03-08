using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DemoItemRepository : IDemoItemRepository
{
    private readonly AppDbContext _db;

    public DemoItemRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<DemoItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.DemoItems.OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }

    public async Task<DemoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.DemoItems.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<DemoItem> AddAsync(DemoItem entity, CancellationToken cancellationToken = default)
    {
        _db.DemoItems.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<DemoItem?> UpdateAsync(int id, Action<DemoItem> update, CancellationToken cancellationToken = default)
    {
        var item = await _db.DemoItems.FindAsync(new object[] { id }, cancellationToken);
        if (item is null) return null;
        update(item);
        await _db.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _db.DemoItems.FindAsync(new object[] { id }, cancellationToken);
        if (item is null) return false;
        _db.DemoItems.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
