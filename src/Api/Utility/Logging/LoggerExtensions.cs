using Serilog.Context;

namespace Api.Utility.Logging;

public static class LoggerExtensions
{
    public static void LogAppInformation(this ILogger logger, string messageId, string message, params object?[] args) =>
        Write(logger, LogLevel.Information, messageId, message, null, args);

    public static void LogAppWarning(this ILogger logger, string messageId, string message, params object?[] args) =>
        Write(logger, LogLevel.Warning, messageId, message, null, args);

    public static void LogAppError(this ILogger logger, string messageId, string message, params object?[] args) =>
        Write(logger, LogLevel.Error, messageId, message, null, args);

    public static void LogAppError(this ILogger logger, string messageId, Exception exception, string message, params object?[] args) =>
        Write(logger, LogLevel.Error, messageId, message, exception, args);

    public static void LogAppCritical(this ILogger logger, string messageId, string message, params object?[] args) =>
        Write(logger, LogLevel.Critical, messageId, message, null, args);

    public static void LogAppCritical(this ILogger logger, string messageId, Exception exception, string message, params object?[] args) =>
        Write(logger, LogLevel.Critical, messageId, message, exception, args);

    private static void Write(
        ILogger logger,
        LogLevel level,
        string messageId,
        string message,
        Exception? exception,
        object?[] args)
    {
        using (LogContext.PushProperty("MessageId", messageId))
        {
            logger.Log(level, exception, message, args);
        }
    }
}
