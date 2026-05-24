namespace Api.Models;

public class MessageContainer
{
    public List<ApiMessageItem> NrmList { get; set; } = [];
    public List<ApiMessageItem> WrnList { get; set; } = [];
    public List<ApiMessageItem> ErrList { get; set; } = [];
}
