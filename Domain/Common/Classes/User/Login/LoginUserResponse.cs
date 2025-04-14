using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Login;

public class LoginUserResponse : IResponse
{
    public required string Token { get; set; }
}