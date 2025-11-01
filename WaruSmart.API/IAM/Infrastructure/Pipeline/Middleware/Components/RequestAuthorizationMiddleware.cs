using WaruSmart.API.IAM.Application.Internal.OutboundServices;
using WaruSmart.API.IAM.Domain.Model.Queries;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService, ITokenService tokenService)
    {
        Console.WriteLine("Entering InvokeAsync");
        // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.Request.HttpContext.GetEndpoint()!
            .Metadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute));
        Console.WriteLine($"Allow Anonymous is {allowAnonymous}");
        if (allowAnonymous)
        {
            Console.WriteLine("Skipping authorization");
            // [AllowAnonymous] attribute is set, so skip authorization
            await next(context);
            return;
        }
        Console.WriteLine("Entering authorization");
        try
        {
            // Get token from request header
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            
            // If token is null then return 401 Unauthorized
            if (token is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Authorization token is required" });
                return;
            }
            
            // Validate token
            var userId = await tokenService.ValidateToken(token);
            
            // If token is invalid then return 401 Unauthorized
            if (userId is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid token" });
                return;
            }
            
            // Create a GetUserByIdQuery object
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
            
            // Get the user by id through the userQueryService
            var user = await userQueryService.Handle(getUserByIdQuery);
            
            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "User not found" });
                return;
            }
            
            // Set the user in HTTP Context
            Console.WriteLine("Successful authorization. Updating Context...");
            context.Items["User"] = user;
            
            // Continue with the request pipeline
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Authorization error: {ex.Message}");
            
            // Return 500 Internal Server Error
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "An error occurred while processing the request" });
        }
        
    }
}