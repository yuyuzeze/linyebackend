namespace Api.Utility.Messages;

public static class MessageTemplateServiceCollectionExtensions
{
    public static IServiceCollection AddMessageTemplates(this IServiceCollection services)
    {
        services.AddSingleton<IMessageTemplateService, MessageTemplateService>();
        return services;
    }
}
