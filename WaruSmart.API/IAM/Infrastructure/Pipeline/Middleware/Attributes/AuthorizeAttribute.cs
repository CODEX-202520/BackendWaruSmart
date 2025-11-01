using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.ValueObjects;

namespace WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute: Attribute, IAuthorizationFilter
{
    private readonly string? _requiredRole;

    public AuthorizeAttribute()
    {
        _requiredRole = null;
    }

    public AuthorizeAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            Console.WriteLine("Skipping authorization");
            return;
        }
        
        var user = (User?)context.HttpContext.Items["User"];
        if (user is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check if a specific role is required
        if (!string.IsNullOrEmpty(_requiredRole))
        {
            if (user.Role.ToString() != _requiredRole)
            {
                context.Result = new JsonResult(new { message = "Forbidden: Insufficient permissions" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }
    }
}