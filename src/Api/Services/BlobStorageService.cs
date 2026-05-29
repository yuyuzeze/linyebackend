using Api.Models.Dtos;
using Api.Interfaces;
using Azure.Storage.Blobs;

namespace Api.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;

    public BlobStorageService(IConfiguration configuration)
    {
        var connStr = configuration["BlobStorage:ConnectionString"] ?? "";
        _client = new BlobServiceClient(connStr);
    }

    public async Task<BlobListResultDto> ListAsync(string containerName, string? prefix, CancellationToken cancellationToken = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        var prefixes = new List<BlobItemDto>();
        var items = new List<BlobItemDto>();

        await foreach (var page in container.GetBlobsByHierarchyAsync(prefix: prefix ?? "", delimiter: "/", cancellationToken: cancellationToken))
        {
            if (page.IsPrefix)
            {
                var segment = page.Prefix.TrimEnd('/').Split('/').LastOrDefault() ?? page.Prefix;
                prefixes.Add(new BlobItemDto(segment, true, null, null));
            }
            else if (page.Blob != null)
                items.Add(new BlobItemDto(page.Blob.Name, false, page.Blob.Properties?.LastModified, page.Blob.Properties?.ContentLength ?? 0));
        }

        return new BlobListResultDto(prefixes, items);
    }
}
