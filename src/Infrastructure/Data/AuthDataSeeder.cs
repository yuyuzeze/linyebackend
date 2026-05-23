using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public static class AuthDataSeeder
{
    public static async Task SeedAsync(
        AppDbContext db,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var pending = await db.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pending.Any())
            await db.Database.MigrateAsync(cancellationToken);

        if (await db.Departments.AnyAsync(cancellationToken))
            return;

        var dept = new Department { Code = "DEV", Name = "開発部門" };
        db.Departments.Add(dept);
        await db.SaveChangesAsync(cancellationToken);

        var devOid = configuration["Authentication:DevUser:ObjectId"];
        if (!string.IsNullOrWhiteSpace(devOid))
        {
            db.UserRoles.Add(new UserRole
            {
                EntraObjectId = devOid,
                Upn = configuration["Authentication:DevUser:Upn"] ?? "dev@local.test",
                DepartmentId = dept.Id,
                RoleCode = AppRoleCodes.All,
                IsActive = true
            });
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seeded dev UserRole for OID {ObjectId}", devOid);
        }

        logger.LogInformation("Seeded default Department DEV");
    }
}
