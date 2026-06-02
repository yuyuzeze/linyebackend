using System.Security.Claims;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApiInfrastructure(this WebApplicationBuilder builder)
    {
        var vaultUri = builder.Configuration["KeyVault:VaultUri"];
        if (!string.IsNullOrWhiteSpace(vaultUri))
        {
            builder.Services.AddSingleton(_ => new SecretClient(new Uri(vaultUri), new DefaultAzureCredential()));
        }

        builder.Services.AddHttpClient("external");
        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    public static WebApplicationBuilder AddApiAuthentication(this WebApplicationBuilder builder)
    {
        var authEnabled = builder.Configuration.GetValue("Authentication:Enabled", false);
        var azureAdSection = builder.Configuration.GetSection("AzureAd");
        var azureClientId = azureAdSection["ClientId"];

        builder.Services.AddAuthorization(options =>
        {
            if (authEnabled && !string.IsNullOrWhiteSpace(azureClientId))
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            }
        });

        if (!authEnabled || string.IsNullOrWhiteSpace(azureClientId))
            return builder;

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(azureAdSection);

        var audience = azureAdSection["Audience"];
        builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;

            // Entra v2 アクセストークンの aud は GUID または api://{guid} のどちらかになる
            var validAudiences = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(audience))
                validAudiences.Add(audience);
            if (!string.IsNullOrWhiteSpace(azureClientId))
            {
                validAudiences.Add(azureClientId);
                validAudiences.Add($"api://{azureClientId}");
            }

            if (validAudiences.Count > 0)
            {
                options.TokenValidationParameters.ValidAudiences = validAudiences.ToList();
                options.TokenValidationParameters.ValidateAudience = true;
            }

            if (builder.Environment.IsDevelopment())
            {
                options.Events ??= new JwtBearerEvents();
                options.Events.OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger("JwtBearer");
                    logger.LogWarning(
                        context.Exception,
                        "JWT 認証に失敗しました。Path={Path}",
                        context.Request.Path);
                    return Task.CompletedTask;
                };
            }
        });

        return builder;
    }
}
