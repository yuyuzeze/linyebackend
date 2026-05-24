namespace Api.Logging;

/// <summary>プレースホルダー メッセージ ID。後日、正式メッセージコード表に置き換える。</summary>
public static class AppMessageIds
{
    public const string Information = "APP-I001";
    public const string HttpRequest = "APP-I010";
    public const string Warning = "APP-W001";
    public const string AuthWarning = "APP-W002";
    public const string BusinessError = "APP-E001";
    public const string ClientReport = "APP-E901";
    public const string UnhandledException = "APP-F001";
}
