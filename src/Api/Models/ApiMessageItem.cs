namespace Api.Models;

public record ApiMessageItem(string Code, string Message, IReadOnlyList<string>? Params = null);
