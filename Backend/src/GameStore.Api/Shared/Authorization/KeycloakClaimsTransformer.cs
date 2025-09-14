using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.Api.Shared.Authorization;

public class KeycloakClaimsTransformer(
    ILogger<KeycloakClaimsTransformer> logger
)
{
    public void Transform(TokenValidatedContext ctx)
    {
        ClaimsIdentity? identity = ctx.Principal?.Identity as ClaimsIdentity;
        Claim? scopeClaim = identity?.FindFirst(ClaimTypes.Scope);

        if (scopeClaim is null)
        {
            return;
        }

        string[] scopes = scopeClaim.Value.Split(' ');
        identity?.RemoveClaim(scopeClaim);
        identity!.AddClaims(
            scopes.Select(
                scope => new Claim(ClaimTypes.Scope, scope)
            )
        );

        IEnumerable<Claim>? claims = ctx.Principal?.Claims;
        if (claims is null)
        {
            return;
        }

        foreach (var claim in claims)
        {
            logger.LogTrace("claim: {ClaimType}, value: {ClaimValue}",
                claim.Type,
                claim.Value
            );
        }
    }
}
