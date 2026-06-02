namespace Api.Utility.Storage;

/// <summary>
/// Azure Blob Storage の共通操作（一覧・読取・保存）。
/// </summary>
public interface IBlobStorageService
{
    /// <summary>指定プレフィックス配下のフォルダ・ファイル一覧を取得する。</summary>
    Task<BlobListResult> ListAsync(string containerName, string? prefix, CancellationToken cancellationToken = default);

    /// <summary>Blob をストリームで読み取る。呼び出し側で <see cref="BlobDownload.Content"/> を破棄すること。</summary>
    Task<BlobDownload> OpenReadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

    /// <summary>ストリームを Blob に保存する。</summary>
    Task UploadAsync(
        string containerName,
        string blobName,
        Stream content,
        string? contentType = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default);

    /// <summary>Blob が存在するか確認する。</summary>
    Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
}
