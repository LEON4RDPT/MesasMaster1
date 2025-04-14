using Domain.Common.Classes.Jwt;

namespace Application.Interfaces.Auth;

public interface IGenerateToken
{
    string Generate(JwtUserRequest user);

}