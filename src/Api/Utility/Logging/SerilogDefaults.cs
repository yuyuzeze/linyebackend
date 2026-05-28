namespace Api.Utility.Logging;

public static class SerilogDefaults
{
    public const string OutputTemplate =
        "{Timestamp:yyyy-MM-ddTHH:mm:ss.fffZ} {Level:u3} {MachineName} {SourceContext} [{MessageId}] {Message} UPN={Upn} ReqId={RequestId}{NewLine}{Exception}";

    public const int AppLogFileSizeBytes = 20 * 1024 * 1024;
    public const int DebugLogFileSizeBytes = 100 * 1024 * 1024;
}
