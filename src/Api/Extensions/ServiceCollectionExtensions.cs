using Api.Interfaces;
using Api.Models.Mapping;
using Api.Services;
using Api.Utility.Messages;
using Api.Utility.Storage;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.DataAccess;
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
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IQueryGateway, DapperQueryGateway>();

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDemoItemService, DemoItemService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<Services.Satei.SateiSampleService>();
        services.AddMessageTemplates();
        services.AddBlobStorage(configuration);

        return services;
    }
}
