using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ApplicationTypeRepository : IApplicationTypeRepository
{
    private readonly AppDbContext _db;

    public ApplicationTypeRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ApplicationType>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.ApplicationTypes.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<ApplicationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.ApplicationTypes.FindAsync(new object[] { id }, cancellationToken);
}
