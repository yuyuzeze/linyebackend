using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        return builder;
    }

    public static WebApplicationBuilder AddApiAuthentication(this WebApplicationBuilder builder)
    {
        var authBuilder = builder.Services.AddAuthentication();

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var jwtAuthority = jwtSection["Authority"];
        var jwtAudience = jwtSection["Audience"];
        if (!string.IsNullOrWhiteSpace(jwtAuthority) || !string.IsNullOrWhiteSpace(jwtAudience))
        {
            authBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                if (!string.IsNullOrWhiteSpace(jwtAuthority))
                    options.Authority = jwtAuthority;
                if (!string.IsNullOrWhiteSpace(jwtAudience))
                    options.Audience = jwtAudience;
            });
        }

        var azureAdSection = builder.Configuration.GetSection("AzureAd");
        if (!string.IsNullOrWhiteSpace(azureAdSection["ClientId"]))
        {
            authBuilder.AddMicrosoftIdentityWebApi(azureAdSection);
        }

        builder.Services.AddAuthorization();
        return builder;
    }
}
