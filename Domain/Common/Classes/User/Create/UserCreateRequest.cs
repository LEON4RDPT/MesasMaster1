using Domain.Common.Interfaces;

namespace Domain.Common.Classes.User.Create;

public class UserCreateRequest : IRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}