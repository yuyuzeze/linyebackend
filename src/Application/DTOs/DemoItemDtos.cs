namespace Application.DTOs;

public record DemoItemDto(int Id, string Name, string? Description, DateTime CreatedAt);

public record CreateDemoItemDto(string Name, string? Description);

public record UpdateDemoItemDto(string Name, string? Description);
