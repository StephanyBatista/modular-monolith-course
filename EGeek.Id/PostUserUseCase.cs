using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id;

public static class PostUserUseCase
{
    public static async Task<Created<string>> Action(
        PostUserRequest request,
        UserManager<User> userManager)
    {
        var user = new User(request);

        var result = await userManager.CreateAsync(user, user.PasswordToSave);
        if(!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);
        
        result = await userManager.AddClaimAsync(user, user.RoleClaim);
        if(!result.Succeeded)
            throw new ApplicationException("Failed to add claim to user.");
        
        return TypedResults.Created($"/v1/users/{user.Id}", user.Id);
    }
}