namespace GameStore.Api.Features.Baskets.Authorization;

/// <summary>
/// Authorization handler. A class responsible for evaluating
/// the authorization requirements's properties.
///
/// A resource-based handler is an authorization handler that
/// specifies both a requirement and a resource type.
/// </summary>
public class BasketAuthorizationHandler : AuthorizationHandler<
    OwnerOrAdminRequirement,
    CustomerBasket>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement,
        CustomerBasket resource
    )
    {
        string? currentUserId = context.User
            .FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Task.CompletedTask;
        }

        bool ok = Guid.TryParse(currentUserId, out Guid parsedUserId);
        if (!ok)
        {
            return Task.CompletedTask;
        }

        bool isUser = resource.Id == parsedUserId;
        bool isAdmin = context.User.IsInRole(Roles.Admin);

        ok = isUser || isAdmin;

        if (ok)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

// Authorization requirement. A collection of data parameters that a policy
// can use to evaludate the current use principal.

public class OwnerOrAdminRequirement : IAuthorizationRequirement
{
}
