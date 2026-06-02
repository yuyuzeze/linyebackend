namespace Api.Utility.Storage;

public static class BlobStorageServiceCollectionExtensions
{
    public static IServiceCollection AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BlobStorageOptions>(configuration.GetSection(BlobStorageOptions.SectionName));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        return services;
    }
}
