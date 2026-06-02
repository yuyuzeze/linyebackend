using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Api.Utility.Storage;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;

    public BlobStorageService(IOptions<BlobStorageOptions> options)
    {
        var connStr = options.Value.ConnectionString ?? "";
        _client = new BlobServiceClient(connStr);
    }

    public async Task<BlobListResult> ListAsync(string containerName, string? prefix, CancellationToken cancellationToken = default)
    {
        try
        {
            var container = _client.GetBlobContainerClient(containerName);
            var prefixes = new List<BlobItemInfo>();
            var items = new List<BlobItemInfo>();

            await foreach (var page in container.GetBlobsByHierarchyAsync(
                               prefix: prefix ?? "",
                               delimiter: "/",
                               cancellationToken: cancellationToken))
            {
                if (page.IsPrefix)
                {
                    var segment = page.Prefix.TrimEnd('/').Split('/').LastOrDefault() ?? page.Prefix;
                    prefixes.Add(new BlobItemInfo(segment, true, null, null));
                }
                else if (page.Blob != null)
                    items.Add(new BlobItemInfo(
                        page.Blob.Name,
                        false,
                        page.Blob.Properties?.LastModified,
                        page.Blob.Properties?.ContentLength ?? 0));
            }

            return new BlobListResult(prefixes, items);
        }
        catch (RequestFailedException ex) when (ex.Status == 404 || ex.ErrorCode == "ContainerNotFound")
        {
            throw new BlobContainerNotFoundException(containerName);
        }
    }

    public async Task<BlobDownload> OpenReadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
            var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
            var props = response.Value.Details;
            var contentType = string.IsNullOrWhiteSpace(props.ContentType)
                ? "application/octet-stream"
                : props.ContentType;

            return new BlobDownload(
                response.Value.Content,
                contentType,
                props.ContentLength,
                blobName);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            if (ex.ErrorCode is "ContainerNotFound")
                throw new BlobContainerNotFoundException(containerName);
            throw new BlobNotFoundException(containerName, blobName);
        }
    }

    public async Task UploadAsync(
        string containerName,
        string blobName,
        Stream content,
        string? contentType = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
            var headers = string.IsNullOrWhiteSpace(contentType)
                ? null
                : new BlobHttpHeaders { ContentType = contentType };

            await blob.UploadAsync(
                content,
                new BlobUploadOptions
                {
                    HttpHeaders = headers,
                    Conditions = overwrite ? null : new BlobRequestConditions { IfNoneMatch = new ETag("*") }
                },
                cancellationToken);
        }
        catch (RequestFailedException ex) when (ex.Status == 404 && ex.ErrorCode == "ContainerNotFound")
        {
            throw new BlobContainerNotFoundException(containerName);
        }
    }

    public async Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
            return await blob.ExistsAsync(cancellationToken);
        }
        catch (RequestFailedException ex) when (ex.Status == 404 && ex.ErrorCode == "ContainerNotFound")
        {
            throw new BlobContainerNotFoundException(containerName);
        }
    }
}
