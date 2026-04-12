using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ApplicationTypeFieldRepository : IApplicationTypeFieldRepository
{
    private readonly AppDbContext _db;

    public ApplicationTypeFieldRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ApplicationTypeField>> GetByApplicationTypeIdAsync(int applicationTypeId, CancellationToken cancellationToken = default)
        => await _db.ApplicationTypeFields.Where(x => x.ApplicationTypeId == applicationTypeId).OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id).ToListAsync(cancellationToken);
}
