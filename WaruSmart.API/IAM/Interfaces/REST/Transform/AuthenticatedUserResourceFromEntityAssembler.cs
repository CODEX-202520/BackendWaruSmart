using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Interfaces.REST.Resources;

namespace WaruSmart.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User entity, string token)
    {
        return new AuthenticatedUserResource(entity.Id, entity.Username, entity.Role.ToString(), token);
    } 
}