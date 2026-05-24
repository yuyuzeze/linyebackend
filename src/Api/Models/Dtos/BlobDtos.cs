namespace Api.Models.Dtos;

public record BlobItemDto(string Name, bool IsPrefix, DateTimeOffset? LastModified, long? Length);

public record BlobListResultDto(IReadOnlyList<BlobItemDto> Prefixes, IReadOnlyList<BlobItemDto> Items);
