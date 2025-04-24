using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Auth;
using Application.UseCases.Jwt;
using Domain.Exceptions.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Auth;

public class GenerateToken(IConfiguration configuration) : IGenerateToken
{
    public string Generate(JwtUserRequest user)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new MissingEnvironmentValue("jwt:key"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new("UserId", user.Id.ToString()),
            new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}