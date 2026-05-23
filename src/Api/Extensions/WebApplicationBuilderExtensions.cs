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

        builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
        });

        return builder;
    }
}
