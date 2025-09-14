namespace GameStore.Api.Shared.Authorization;

public static class AuthorizationExtensions
{
    public static IHostApplicationBuilder AddGameStoreAuthentication(
        this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<KeycloakClaimsTransformer>();

        builder.Services.AddAuthentication(Schemes.Keycloak)
            .AddJwtBearer(opt =>
            {
                opt.MapInboundClaims = false;
                opt.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
            })
            // this JwtBearer will be selected
            .AddJwtBearer(Schemes.Keycloak, opt =>
            {
                opt.MapInboundClaims = false;
                opt.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                opt.RequireHttpsMetadata = false;
                opt.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        var transformer = ctx.HttpContext
                            .RequestServices
                            .GetRequiredService<KeycloakClaimsTransformer>();
                        transformer.Transform(ctx);

                        return Task.CompletedTask;
                    }
                };
            });

        return builder;
    }

    public static IHostApplicationBuilder AddGameStoreAuthorization(
        this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddFallbackPolicy(Policies.UserAccess, authBuilder =>
            {
                authBuilder.RequireClaim(ClaimTypes.Scope, Claims.ApiAccessScope);
            })
            .AddPolicy(Policies.AdminAccess, authBuilder =>
            {
                authBuilder.RequireClaim(ClaimTypes.Scope, Claims.ApiAccessScope);
                authBuilder.RequireRole(Roles.Admin);
            });

        return builder;
    }
}
