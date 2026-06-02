namespace Api.Utility.Storage;

public record BlobItemInfo(string Name, bool IsPrefix, DateTimeOffset? LastModified, long? Length);

public record BlobListResult(IReadOnlyList<BlobItemInfo> Prefixes, IReadOnlyList<BlobItemInfo> Items);

public record BlobDownload(Stream Content, string ContentType, long? ContentLength, string BlobName);
