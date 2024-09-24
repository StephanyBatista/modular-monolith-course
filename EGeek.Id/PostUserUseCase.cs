using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id;

public static class PostUserUseCase
{
    public static async Task<Created<string>> Action(
        CreateUserRequest request,
        UserManager<User> userManager)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required");
        if(string.IsNullOrEmpty(request.Password))
            throw new ArgumentException("Password is required");
        if(string.IsNullOrEmpty(request.Role))
            throw new ArgumentException("Role is required");

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if(!result.Succeeded)
            throw new ApplicationException(result.Errors.First().Description);
        
        var roleClaim = new Claim(ClaimTypes.Role, request.Role);
        result = await userManager.AddClaimAsync(user, roleClaim);
        if(!result.Succeeded)
            throw new ApplicationException("Failed to add claim to user.");
        
        return TypedResults.Created($"/v1/users/{user.Id}", user.Id);
    }
}