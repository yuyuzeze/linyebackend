namespace Api.Utility.Storage;

public class BlobContainerNotFoundException : Exception
{
    public BlobContainerNotFoundException(string containerName)
        : base($"Blob container not found: {containerName}")
    {
        ContainerName = containerName;
    }

    public string ContainerName { get; }
}

public class BlobNotFoundException : Exception
{
    public BlobNotFoundException(string containerName, string blobName)
        : base($"Blob not found: {containerName}/{blobName}")
    {
        ContainerName = containerName;
        BlobName = blobName;
    }

    public string ContainerName { get; }
    public string BlobName { get; }
}
