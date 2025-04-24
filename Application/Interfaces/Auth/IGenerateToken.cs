using Application.UseCases.Jwt;

namespace Application.Interfaces.Auth;

public interface IGenerateToken
{
    string Generate(JwtUserRequest user);
}