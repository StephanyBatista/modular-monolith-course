using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id;

internal class User : IdentityUser
{
    [NotMapped]
    public string PasswordToSave { get; private set; }
    [NotMapped]
    public Claim RoleClaim { get; private set; }

    private User()
    {
        PasswordToSave = string.Empty;
        RoleClaim = new Claim(ClaimTypes.Role, string.Empty);
    }
    
    public User(PostUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required");
        if(string.IsNullOrEmpty(request.Role))
            throw new ArgumentException("Role is required");
        if(string.IsNullOrEmpty(request.Password))
            throw new ArgumentException("Password is required");
        if(request.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long");
        
        var hasUpperCase = new Regex(@"[A-Z]");
        var hasNumber = new Regex(@"[0-9]");
        var hasSpecialChar = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]");

        var passwordIsValid = hasUpperCase.IsMatch(request.Password) &&
               hasNumber.IsMatch(request.Password) &&
               hasSpecialChar.IsMatch(request.Password);
        
        if(!passwordIsValid)
            throw new ArgumentException(
                "Password must have at least one uppercase letter, one number and one special character");
        
        UserName = request.Email;
        Email = request.Email;
        PasswordToSave = request.Password;
        RoleClaim = new Claim(ClaimTypes.Role, request.Role);
    }
}