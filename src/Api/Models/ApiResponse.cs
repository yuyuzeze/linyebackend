namespace Api.Models;

public class ApiResponse<T>
{
    public T? Result { get; set; }
    public MessageContainer Messages { get; set; } = new();
    public string? StatusDetailMessage { get; set; }
    public int StatusCode { get; set; }
}
