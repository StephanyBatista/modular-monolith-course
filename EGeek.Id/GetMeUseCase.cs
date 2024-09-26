using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id;

internal static class GetMeUseCase
{
    [Authorize]
    public static async Task<Ok<GetMeResponse>> Action(
        ClaimsPrincipal principal, 
        UserManager<User> userManager)
    {
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var user = await userManager.FindByEmailAsync(email!);
        var claims = await userManager.GetClaimsAsync(user!);
        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return TypedResults.Ok(new GetMeResponse(user!.Id, email!, role!));
    }
}