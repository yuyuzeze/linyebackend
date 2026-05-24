namespace Api.Models.Dtos;

public class ClientLogEntryDto
{
    public string Level { get; set; } = "Error";
    public string? MessageId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Stack { get; set; }
}
