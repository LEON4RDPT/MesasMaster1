using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Login;

public class LoginUserRequest : IRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}