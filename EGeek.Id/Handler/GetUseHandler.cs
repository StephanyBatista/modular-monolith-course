using System.Security.Claims;
using EGeek.Id.Contract;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id.Handler;

internal class GetUseHandler(UserManager<User> userManager)
    : IRequestHandler<GetUserQuery, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request?.Email))
            throw new ArgumentException("Email is required");
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if(user == null)
            throw new ArgumentException("User not found");
        var claims = await userManager.GetClaimsAsync(user!);
        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return new GetUserResponse(user.Id, user.Email, role);
    }
}