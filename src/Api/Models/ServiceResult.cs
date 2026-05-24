namespace Api.Models;

public sealed class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public int StatusCode { get; init; } = StatusCodes.Status200OK;
    public MessageContainer Messages { get; init; } = new();
    public string? StatusDetailMessage { get; init; }

    public static ServiceResult<T> Success(
        T data,
        int statusCode = StatusCodes.Status200OK,
        params ApiMessageItem[] nrmMessages) =>
        new()
        {
            IsSuccess = true,
            Data = data,
            StatusCode = statusCode,
            Messages = new MessageContainer { NrmList = nrmMessages.ToList() }
        };

    public static ServiceResult<T> OkWithWarning(
        T data,
        ApiMessageItem warning,
        params ApiMessageItem[] nrmMessages) =>
        new()
        {
            IsSuccess = true,
            Data = data,
            StatusCode = StatusCodes.Status200OK,
            Messages = new MessageContainer
            {
                NrmList = nrmMessages.ToList(),
                WrnList = [warning]
            }
        };

    public static ServiceResult<T> Failure(
        int statusCode,
        params ApiMessageItem[] errMessages) =>
        new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Messages = new MessageContainer { ErrList = errMessages.ToList() }
        };

    public static ServiceResult<T> NotFound(string message, string code = "EKYOTSU40401") =>
        Failure(StatusCodes.Status404NotFound, new ApiMessageItem(code, message));
}
