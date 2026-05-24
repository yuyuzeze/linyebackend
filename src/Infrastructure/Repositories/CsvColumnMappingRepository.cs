using Infrastructure.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CsvColumnMappingRepository : ICsvColumnMappingRepository
{
    private readonly AppDbContext _db;

    public CsvColumnMappingRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<CsvColumnMapping>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default)
        => await _db.CsvColumnMappings.Where(x => x.ApplicationTypeId == applicationTypeId).ToListAsync(cancellationToken);
}
