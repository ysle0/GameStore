namespace GameStore.Api.Shared.Authorization;

public static class AuthorizationExtensions
{
    public static IHostApplicationBuilder AddGameStoreAuthentication(
        this IHostApplicationBuilder builder)
    {
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
