using System.Globalization;
using System.Text.Json;

namespace Api.Utility.Messages;

public class MessageTemplateService : IMessageTemplateService
{
    private readonly Lazy<Dictionary<string, string>> _catalog;

    public MessageTemplateService(IWebHostEnvironment environment)
    {
        _catalog = new Lazy<Dictionary<string, string>>(() => LoadCatalog(environment));
    }

    public string Format(string code, params object?[] args)
    {
        var template = _catalog.Value.TryGetValue(code, out var value) ? value : code;
        if (args.Length == 0 || !template.Contains('{'))
            return template;

        try
        {
            return string.Format(CultureInfo.InvariantCulture, template, args);
        }
        catch (FormatException)
        {
            return template;
        }
    }

    private static Dictionary<string, string> LoadCatalog(IWebHostEnvironment environment)
    {
        var path = Path.Combine(environment.ContentRootPath, "Resources", "messages.ja.json");
        if (!File.Exists(path))
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var json = File.ReadAllText(path);
        var map = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        return map is null
            ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(map, StringComparer.OrdinalIgnoreCase);
    }
}
