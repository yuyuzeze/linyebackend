using Api.Models.Dtos;
using Api.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Api.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;
    private readonly string _defaultContainer;
    private readonly string _defaultPrefix;

    public BlobStorageService(IConfiguration configuration)
    {
        var connStr = configuration["BlobStorage:ConnectionString"] ?? "";
        _defaultContainer = configuration["BlobStorage:UploadContainerName"] ?? "csv-inbox";
        _defaultPrefix = configuration["BlobStorage:UploadPrefix"] ?? "uploads/";
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

    public async Task<Stream> GetContentAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);
        var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }
}
