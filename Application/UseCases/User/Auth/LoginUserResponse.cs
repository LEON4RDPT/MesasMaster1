using Application.Interfaces;

namespace Application.UseCases.User.Auth;

public class LoginUserResponse : IResponse
{
    public required string Token { get; set; }
}