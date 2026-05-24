using Api.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);
        return services;
    }
}
