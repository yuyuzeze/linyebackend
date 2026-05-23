namespace Application.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? ObjectId { get; }
    string? Upn { get; }
    string? DisplayName { get; }
}
