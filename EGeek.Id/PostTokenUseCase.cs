using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EGeek.Id;

internal static class PostTokenUseCase
{
    public static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Action(
        PostTokenRequest request, 
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        if(string.IsNullOrEmpty(request.Password))
            throw new ArgumentException("Password is required");
        if(string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required");
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return TypedResults.Unauthorized();
        
        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            return TypedResults.Unauthorized();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            [new Claim(ClaimTypes.Email, request.Email)],
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
        
        return TypedResults.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}