namespace GameStore.Api.Shared.Authorization;

public static class AuthorizationExtensions
{
    public static IHostApplicationBuilder
        AddGameStoreAuthorization(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(Policies.UserAccess, authBuilder =>
            {
                authBuilder.RequireClaim(Claims.Scope, Claims.ApiAccessScope);
            })
            .AddPolicy(Policies.AdminAccess, authBuilder =>
            {
                authBuilder.RequireClaim(Claims.Scope, Claims.ApiAccessScope);
                authBuilder.RequireRole(Roles.Admin);
            });

        return builder;
    }
}
