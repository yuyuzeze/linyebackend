namespace Api.Utility.Messages;

public interface IMessageTemplateService
{
    string Format(string code, params object?[] args);
}
