using System.Security.Claims;
using EGeek.Id.Contract;
using MediatR;

namespace EGeek.Catalog;

public class RoleValidator(IMediator mediator)
{
    public async Task<bool> Validate(ClaimsPrincipal principal)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;

        if (email is null)
            return false;
        
        var query = new GetUserQuery(email);
        var result = await mediator.Send(query);

        return result.Role == "catalog";
    }
}