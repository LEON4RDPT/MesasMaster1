using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Auth;

public class LoginUserResponse : IResponse
{
    public required string Token { get; set; }
}