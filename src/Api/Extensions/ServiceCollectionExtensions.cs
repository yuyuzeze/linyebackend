using Api.Interfaces;
using Api.Models.Mapping;
using Api.Services;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=linye;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IDemoItemRepository, DemoItemRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IProcessedBlobRecordRepository, ProcessedBlobRecordRepository>();
        services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();
        services.AddScoped<IApplicationTypeFieldRepository, ApplicationTypeFieldRepository>();
        services.AddScoped<ICsvColumnMappingRepository, CsvColumnMappingRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IDemoItemService, DemoItemService>();
        services.AddScoped<IVoucherImportService, VoucherImportService>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IApplicationTypeService, ApplicationTypeService>();
        services.AddScoped<ICsvMappingService, CsvMappingService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
