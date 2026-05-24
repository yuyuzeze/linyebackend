using Api.Models.Dtos;

namespace Api.Interfaces;

public interface IBlobStorageService
{
    Task<BlobListResultDto> ListAsync(string containerName, string? prefix, CancellationToken cancellationToken = default);
    Task<Stream> GetContentAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
}
